using System.Drawing;
using Microsoft.AspNetCore.Http;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class ImageFileResult
    {
        public StorageType StorageType { get;}
        public string Url{get;}
        public string Error{get;}
        public string PublicId{get;}

        public ImageFileResult(string url = null, string error = null, string publicId = null){
            this.Url = url;
            this.Error = error;
            this.PublicId = publicId;
        }
    }
    public interface IImageFileManager
    {
        StorageType GetStorageType();
        ImageFileResult SaveImageFromUrl(string url, string fileName, string targetFolder = null, int maxWidth = 600);
        ImageFileResult SaveImage(IFormFile file, string prefix, string targetFolder = null, int maxWidth = 600);
        ImageFileResult DeleteImage(string publicId);
    }
}