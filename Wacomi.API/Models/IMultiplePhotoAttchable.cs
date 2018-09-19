using System.Collections.Generic;
using Wacomi.API.Data;
using Wacomi.API.Helper;

namespace Wacomi.API.Models
{
    public interface IMultiplePhotoAttchable
    {
        ICollection<Photo> Photos{ get; set;}
         Photo MainPhoto{ get; set;}
        int? MainPhotoId{ get; set;}
    }

    public static class IMultiplePhotoAttchableMethods {
        public static List<string> DeletePhotos(this IMultiplePhotoAttchable dataRecord, ImageFileStorageManager imageFileStorageManager, IRepositoryBase repo) {
            List<string> errors = new List<string>();
            foreach (var photo in dataRecord.Photos)
            {
                repo.Delete(photo);
                var deletingResult = imageFileStorageManager.DeleteImageFile(photo);
                if (!string.IsNullOrEmpty(deletingResult.Error))
                    errors.Add(deletingResult.Error);
            }
            return errors;
        }
    }
}