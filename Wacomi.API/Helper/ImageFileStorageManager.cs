using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class ImageFileStorageManager
    {
        private IDataRepository _repo;
        private IOptions<CloudinarySettings> _cloudinaryConfig;
        public int MaxWidth{ get; set;}

        public ImageFileStorageManager(IServiceProvider serviceProvider,  IOptions<CloudinarySettings> cloudinaryConfig = null){
            _cloudinaryConfig = cloudinaryConfig;
            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScopeFactory.CreateScope();
            this._repo = scope.ServiceProvider.GetService<IDataRepository>();
        }

        public StorageType GetStorageType(string recordType){
            switch(recordType.ToLower()){
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

        private IImageFileManager GetImageFileManager(StorageType storageType){
             IImageFileManager imageFileManager = null;
             switch(storageType){
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
            if(imageFileManager != null)
                return imageFileManager.SaveImageFromUrl(url, fileName, targetFolder, this.MaxWidth);

            return new ImageFileResult(null, "Failed to getting imge file manager");
        }
        public ImageFileResult SaveImage(string recordType, int recordId, IFormFile file, string targetFolder = null){
            try{
                using (var image = System.Drawing.Image.FromStream(file.OpenReadStream())) //For validation
                {
                    string prefix = recordType + "_" + recordId + "_";
                    var storageType = GetStorageType(recordType);
                    IImageFileManager imageFileManager = GetImageFileManager(storageType);
                    if(imageFileManager == null)
                        return new ImageFileResult(null, "Failed to getting imge file manager");

                    return imageFileManager.SaveImage(file, prefix, targetFolder);
                }
            }
            catch(System.Exception ex){
                return new ImageFileResult(null, null, ex.Message);
            }
        }
        public ImageFileResult DeleteImageFile(Photo photoFromRepo){
           IImageFileManager imageFileManager = GetImageFileManager(photoFromRepo.StorageType);
           if(imageFileManager != null)
               return imageFileManager.DeleteImage(photoFromRepo.PublicId);

           return new ImageFileResult(null, "Failed to getting imge file manager");
        }
    }
}