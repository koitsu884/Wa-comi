using System.Drawing;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class CloudinaryManager : IImageFileManager
    {
        private readonly CloudinarySettings _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public CloudinaryManager(CloudinarySettings cloudinarySettings){
            this._cloudinaryConfig = cloudinarySettings;

            CloudinaryDotNet.Account acc = new CloudinaryDotNet.Account(
                _cloudinaryConfig.CloudName,
                _cloudinaryConfig.ApiKey,
                _cloudinaryConfig.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public ImageFileResult SaveImageFromUrl(string url, string fileName, string targetFolder, int maxWidth = 600)
        {
            var uploadResult = new ImageUploadResult();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(url),
                Folder = targetFolder.Replace("\\", "/"),
                Transformation = new Transformation().Width(maxWidth).Height(maxWidth).Crop("fit")
            };

            uploadResult = _cloudinary.Upload(uploadParams);
 
            return new ImageFileResult(uploadResult.SecureUri.ToString(), uploadResult.Error?.Message, uploadResult.PublicId);
        }

        public ImageFileResult SaveImage(IFormFile file, string prefix, string targetFolder = null, int maxWidth = 600)
        {
            if(file.Length == 0){
                return new ImageFileResult(null, "No file data");
            }
            var uploadResult = new ImageUploadResult();
            string error = null;

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(prefix + file.FileName, stream),
                    Folder = targetFolder.Replace("\\", "/"),
                   // Transformation = new Transformation().Width(600).Height(600).Crop("fit")
                };

                uploadResult = _cloudinary.Upload(uploadParams);
                if(uploadResult.Error != null)
                    error = uploadResult.Error.Message;
            }

            // photoDto.Url = uploadResult.SecureUri.ToString();
            // photoDto.PublicId = uploadResult.PublicId;
            return new ImageFileResult(uploadResult.SecureUri.ToString(), error, uploadResult.PublicId);
        }
        public ImageFileResult DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            try{
                var result = _cloudinary.Destroy(deleteParams);
                return new ImageFileResult(null, result.Error != null ? result.Error.Message : null);
            }
            catch(System.Exception ex){
                 return new ImageFileResult(null, ex.Message);
            }

        }

        public StorageType GetStorageType()
        {
            return StorageType.Cloudinary;
        }
    }
}