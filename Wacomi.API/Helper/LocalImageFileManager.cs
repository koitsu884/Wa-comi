using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Http;
using Wacomi.API.Models;

namespace Wacomi.API.Helper
{
    public class LocalImageFileManager : IImageFileManager
    {
        private readonly int RESIZE_WIDTH = 400;
        private readonly string staticFolderName = "static";
        public ImageFileResult DeleteImage(string publicId)//= file name
        {
            string error = null;
            string actualStaticFolderPath = Path.Combine(Directory.GetCurrentDirectory(),this.staticFolderName);

            try{
                File.Delete(publicId);
            }
            catch(IOException ex){
                error = ex.Message;
            }

            return new ImageFileResult(null, null, error);
        }

        public StorageType GetStorageType()
        {
            return StorageType.Local;
        }

        public ImageFileResult SaveImage(IFormFile file, string prefix, string targetFolder = null)
        {
            string savedFilePath = null;
            string fileName = prefix + file.FileName;
            string error = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    using (Stream stream = file.OpenReadStream())
                    {
                        // var bitmap = new Bitmap(stream);
                        var image = Image.FromStream(stream);
                        savedFilePath = this.SaveImageToLocalStorage(image, fileName, targetFolder);
                        savedFilePath = savedFilePath.Replace("\\", "/");
                        //stream.Flush();
                    }
                }
            }
            catch (System.Exception ex)
            {
                error = "Save Image Error:" + ex.Message;
            }
            string actualStaticFolderPath = Path.Combine(Directory.GetCurrentDirectory(),this.staticFolderName);

            string publicId = Path.Combine(actualStaticFolderPath, targetFolder, fileName);
            return new ImageFileResult(savedFilePath, error, publicId);
        }

        public ImageFileResult SaveImageFromUrl(string url, string fileName, string targetFolder)
        {
            string savedFilePath = null;
            string error = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    using (Stream stream = client.OpenRead(url))
                    {
                        // var bitmap = new Bitmap(stream);
                        var image = Image.FromStream(stream);
                        savedFilePath = this.SaveImageToLocalStorage(image, fileName, targetFolder);
                        savedFilePath = savedFilePath.Replace("\\", "/");
                       // stream.Flush();
                    }
                }
            }
            catch (System.Exception ex)
            {
                error = "Save Image Error:" + ex.Message;
            }
            string actualStaticFolderPath = Path.Combine(Directory.GetCurrentDirectory(),this.staticFolderName);
            string publicId = Path.Combine(actualStaticFolderPath, targetFolder, fileName);
            return new ImageFileResult(savedFilePath, error, publicId);
        }

        private string SaveImageToLocalStorage(Image image, string fileName, string targetFolder)
        {
            var actualTargetFolderPath = Path.Combine(this.staticFolderName, targetFolder);
            int resizeWidth = RESIZE_WIDTH;
            int resizeHeight = (int)(image.Height * ((double)resizeWidth / (double)image.Width));
            Bitmap resizeBmp = new Bitmap(image, resizeWidth, resizeHeight);

            var fullFileName = Path.Combine(actualTargetFolderPath, fileName);
            //may throw exception
            Directory.CreateDirectory(actualTargetFolderPath);
            resizeBmp.Save(fullFileName);
            return fullFileName;
        }
    }
}