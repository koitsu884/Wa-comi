using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Wacomi.API.Helper
{
    public class StaticFileManager : IStaticFileManager
    {
        private Dictionary<string,string> _staticFileFolders;
        private readonly ILogger<StaticFileManager> _logger;

        public StaticFileManager(ILogger<StaticFileManager> logger){
            this._staticFileFolders = new Dictionary<string, string>();
            this._logger = logger;
        }

        public void DisplayList(){
            if(this._staticFileFolders.Count == 0){
                Console.WriteLine("No data int static file folders");
            }
            foreach(var item in this._staticFileFolders){
                Console.WriteLine(item.ToString());
            }
        }
        public void AddStaticFileFolder(string requestPath, string fileProvider)
        {
            this._staticFileFolders.Add(requestPath, fileProvider);
        }

        public void DeleteFile(string fullFileName)
        {
            if(string.IsNullOrEmpty(fullFileName))
                return;
            // Console.WriteLine("Deleting file:" + this.convertRequestFileNameToPhysicalPath(fullFileName));
            try{
                File.Delete(this.convertRequestFileNameToPhysicalPath(fullFileName));
            }
            catch(Exception ex){
                this._logger.LogError(ex.Message);
            }
        }

        public void SaveImageFile(string fullFileName, Bitmap bitmap, ImageFormat format)
        {
            var physicalFilePath = this.convertRequestFileNameToPhysicalPath(fullFileName);
            var directory = Path.GetDirectoryName(physicalFilePath);
            Directory.CreateDirectory(directory);
            bitmap.Save(physicalFilePath, format);
        }

        private string convertRequestFileNameToPhysicalPath(string fullFileName){
            var fileNameParts = fullFileName.Split('\\', 2);
            var requestPath = fileNameParts[0];
            Console.WriteLine(fullFileName);
            Console.WriteLine(fileNameParts[0]);
            Console.WriteLine(fileNameParts[1]);
            var test = this._staticFileFolders.FirstOrDefault(folder => folder.Key == requestPath);
            Console.Write(test.ToString());
            string pysicalRootPath = this._staticFileFolders.FirstOrDefault(folder => folder.Key == requestPath).Value;
            if(pysicalRootPath == null){
                throw new System.Exception("Error: convertRequestFileNameToPhysicalPath - Could not find request path " + requestPath);
            }
           return Path.Combine(pysicalRootPath, fileNameParts[1]);
        }

        public Dictionary<string, string> GetFolderList()
        {
            return this._staticFileFolders;
        }
    }
}