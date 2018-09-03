using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IPhotoRepository : IRepositoryBase
    {
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetPhotoByPublicId(string publicId);
        //  Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id);
        Task<IEnumerable<Photo>> GetPhotosForAppUser(int id);
        Task<IEnumerable<Photo>> GetPhotosForRecord(string recordType, int recordId);
    }
}