using Microsoft.AspNetCore.Http;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public interface IImageFileManager
    {
        string UploadImage(IFormFile file);
        void DeleteImage(Photo photo);
    }
}