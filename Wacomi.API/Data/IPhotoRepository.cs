using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IPhotoRepository : IRepositoryBase
    {
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetPhotoByPublicId(string publicId);
        //  Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id);
        Task<IEnumerable<Photo>> GetPhotosForAppUser(int id);
        Task<IEnumerable<Photo>> GetPhotosForDbSet(string dbsetName, int recordId);
        Task<Photo> GetPhotoForDbSet(string dbsetName, int recordId);
        // Task<IEnumerable<Photo>> GetPhotosForRecord(string recordType, int recordId);
        Task DeletePhoto(int id);
        Task DeletePhotos(ICollection<Photo> photos);
        Task<int> AddPhotosToRecord(IDataItemWithMultiplePhotos record, List<IFormFile> files);
        Task<int> AddPhotoToRecord(IDataItemWithSinglePhoto record, List<IFormFile> files);
    }
}