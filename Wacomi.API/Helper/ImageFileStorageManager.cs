using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class ImageFileStorageManager
    {
        private IDataRepository _repo;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        public int MaxWidth { get; set; }
        private ILogger<ImageFileStorageManager> _logger { get; }

        public ImageFileStorageManager(IServiceProvider serviceProvider, ILogger<ImageFileStorageManager> logger, IOptions<CloudinarySettings> cloudinaryConfig = null)
        {
            this._logger = logger;
            _cloudinaryConfig = cloudinaryConfig;
            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._repo = scope.ServiceProvider.GetService<IDataRepository>();
        }

        public StorageType GetStorageType(string recordType)
        {
            switch (recordType.ToLower())
            {
                case "attraction":
                    return StorageType.Cloudinary;
                case "attractionreview":
                    return StorageType.Cloudinary;
                case "appuser":
                    return StorageType.Cloudinary;
                case "clanseek":
                    return StorageType.Cloudinary;
                case "blog":
                    return StorageType.Cloudinary;
                case "blogfeed":
                    return StorageType.Local;
            }
            return StorageType.Cloudinary;
        }

        private IImageFileManager GetImageFileManager(StorageType storageType)
        {
            IImageFileManager imageFileManager = null;
            switch (storageType)
            {
                case StorageType.Local:
                    imageFileManager = new LocalImageFileManager();
                    break;
                case StorageType.Cloudinary:
                    imageFileManager = new CloudinaryManager(_cloudinaryConfig.Value);
                    break;
            }
            return imageFileManager;
        }

        public ImageFileResult SaveImageFromUrl(string recordType, string url, string fileName, string targetFolder = null)
        {
            var storageType = GetStorageType(recordType);
            IImageFileManager imageFileManager = GetImageFileManager(storageType);
            if (imageFileManager != null)
                return imageFileManager.SaveImageFromUrl(url, fileName, targetFolder, this.MaxWidth);

            return new ImageFileResult(null, "Failed to getting imge file manager");
        }

        public bool ValidateImage(IFormFile file)
        {
            try
            {
                var image = System.Drawing.Image.FromStream(file.OpenReadStream());
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
        public ImageFileResult SaveImage(string recordType, int recordId, IFormFile file, string targetFolder = null, int maxWidth = 600, bool forceLocalSave = false)
        {
            try
            {
                StorageType storageType;
                string prefix = recordType + "_" + recordId + "_";
                if (forceLocalSave)
                    storageType = StorageType.Local;
                else
                    storageType = GetStorageType(recordType);
                IImageFileManager imageFileManager = GetImageFileManager(storageType);
                if (imageFileManager == null)
                    return new ImageFileResult(null, "Failed to getting imge file manager");

                return imageFileManager.SaveImage(file, prefix, targetFolder, maxWidth);
            }
            catch (System.Exception ex)
            {
                return new ImageFileResult(null, null, ex.Message);
            }
        }
        public ImageFileResult DeleteImageFile(Photo photoFromRepo)
        {
            DeleteThumbsAndIcons(photoFromRepo);
            IImageFileManager imageFileManager = GetImageFileManager(photoFromRepo.StorageType);
            return imageFileManager.DeleteImage(photoFromRepo.PublicId);
        }

        public async Task DeleteImageFileAsync(Photo photoFromRepo)
        {
            DeleteThumbsAndIcons(photoFromRepo);
            IImageFileManager imageFileManager = GetImageFileManager(photoFromRepo.StorageType);
            if (imageFileManager != null)
            {
                var task = await Task.Run(() =>
                {
                    var result = imageFileManager.DeleteImage(photoFromRepo.PublicId);
                    return result;
                });
                if (!string.IsNullOrEmpty(task.Error))
                {
                    Console.WriteLine(task.Error);
                    this._logger.LogError(task.Error);
                }
            }
        }

        private void DeleteThumbsAndIcons(Photo photoFromRepo)
        {
            IImageFileManager imageFileManager = GetImageFileManager(StorageType.Local);
            if (!string.IsNullOrEmpty(photoFromRepo.ThumbnailUrl))
                imageFileManager.DeleteImage(photoFromRepo.ThumbnailUrl);
            if (!string.IsNullOrEmpty(photoFromRepo.IconUrl))
                imageFileManager.DeleteImage(photoFromRepo.IconUrl);
        }

        public List<string> DeleteAttachedPhotos(ICollection<Photo> photos){
            List<string> errors = new List<string>();
            foreach (var photo in photos)
            {
                var deletingResult = DeleteImageFile(photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    errors.Add(deletingResult.Error);
            }
            return errors;
        }

    }
}