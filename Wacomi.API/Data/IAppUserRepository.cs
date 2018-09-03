using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IAppUserRepository : IRepositoryBase
    {
         Task<AppUser> GetAppUser(int id);
         Task<AppUser> GetAppUserByAccountId(string accountId);
         Task AddLikeCountToUser(int userId);
         Task<BusinessProfile> GetBusinessProfile(int id);
        Task<MemberProfile> GetMemberProfile(int id);
        Task<MemberProfile> GetMemberProfileByAccountId(string id);
    }
}