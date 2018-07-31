using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Wacomi.API.Helper
{
    public interface IStaticFileManager
    {
        void AddStaticFileFolder(string requestPath, string fileProvider);
        void SaveImageFile(string fullFileName, Bitmap bitmap, ImageFormat format);
        void DeleteFile(string fullFileName);
        void DisplayList();
        Dictionary<string, string> GetFolderList();
    }
}