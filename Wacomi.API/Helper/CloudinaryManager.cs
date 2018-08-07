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
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public CloudinaryManager(IOptions<CloudinarySettings> cloudinarySettings){
            this._cloudinaryConfig = cloudinarySettings;

            CloudinaryDotNet.Account acc = new CloudinaryDotNet.Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        public string UploadImage(IFormFile file)
        {
            return "";
            // var uploadResult = new ImageUploadResult();

            // if(file.Length > 0){
            //     using (var stream = file.OpenReadStream())
            //     {
            //         var uploadParams = new ImageUploadParams()
            //         {
            //             File = new FileDescription(file.Name, stream),
            //             Transformation = new Transformation().Width(600).Height(600).Crop("fit")
            //         };

            //         uploadResult = _cloudinary.Upload(uploadParams);
            //     }
            // }

            // photoDto.Url = uploadResult.SecureUri.ToString();
            // photoDto.PublicId = uploadResult.PublicId;
        }
                public void DeleteImage(Photo photo)
        {
            
        }
    }
}