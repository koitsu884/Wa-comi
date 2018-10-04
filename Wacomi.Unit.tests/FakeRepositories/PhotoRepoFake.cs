using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.Xunit.MockRepositories
{
    public class PhotoRepoFake : RepositoryBase, IPhotoRepository
    {
        private readonly List<Photo> _photoList;
        private readonly List<AppUser> _appUsers;
        public PhotoRepoFake(ApplicationDbContext context) : base(context)
        {
            _appUsers = CommonTestData.AppUserList;

            _photoList = new List<Photo>(){
                new Photo(){Id = 1, Url="http://apc.jpg"},
                new Photo(){Id = 2, Url="http://bbb.jpg"},
                new Photo(){Id = 3, Url="http://ddd.png"},
            };
        }

        public Task<int> AddPhotosToRecord(IDataItemWithMultiplePhotos record, List<IFormFile> files)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AddPhotoToRecord(IDataItemWithSinglePhoto record, List<IFormFile> files)
        {
            throw new System.NotImplementedException();
        }

        public Task DeletePhoto(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task DeletePhotos(ICollection<Photo> photos)
        {
            throw new System.NotImplementedException();
        }

        public Task<Photo> GetPhoto(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Photo> GetPhotoByPublicId(string publicId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Photo> GetPhotoForDbSet(string dbsetName, int recordId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Photo>> GetPhotosForAppUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Photo>> GetPhotosForDbSet(string dbsetName, int recordId)
        {
            throw new System.NotImplementedException();
        }
    }
}