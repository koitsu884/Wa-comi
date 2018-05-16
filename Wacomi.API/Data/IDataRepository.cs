using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IDataRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();

         Task<IEnumerable<City>> GetCities();
         Task<IEnumerable<HomeTown>> GetHomeTowns();
         Task<Photo> GetPhoto(int id);
         Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id);

        Task<UserBase> GetUser(string type, int id);
        Task<BusinessUser> GetBusinessUser(int id);
        Task<Member> GetMember(int id);
        Task<Member> GetMemberByIdentityId(string id);
        Task<IEnumerable<Member>> GetMembers(UserParams userParams);
        Task<Blog> GetBlog(int id);
        Task<IEnumerable<Blog>> GetBlogsForClass(string className, int id);
    }
}