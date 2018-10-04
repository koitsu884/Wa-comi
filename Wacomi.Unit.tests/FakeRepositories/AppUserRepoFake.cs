using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.Xunit.MockRepositories
{
    public class AppUserRepoFake : RepositoryBase, IAppUserRepository
    {
        private readonly List<AppUser> _appUsers;
        public AppUserRepoFake(ApplicationDbContext context) : base(context)
        {
            _appUsers = CommonTestData.AppUserList;
            _appUsers = new List<AppUser>(){
                new AppUser(){Id = 1, DisplayName="User 1", AccountId="AccountId_1"},
                new AppUser(){Id = 2, DisplayName="User 2", AccountId="AccountId_2"},
                new AppUser(){Id = 3, DisplayName="User 3", AccountId="AccountId_3"},
            };
        }

        public Task AddLikeCountToUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AppUser> GetAppUser(int id)
        {
            return Task.FromResult(_appUsers.FirstOrDefault(au => au.Id == id));
        }

        public Task<AppUser> GetAppUserByAccountId(string accountId)
        {
            return Task.FromResult(_appUsers.FirstOrDefault(a => a.AccountId == accountId));
        }

        public Task<BusinessProfile> GetBusinessProfile(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MemberProfile> GetMemberProfile(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MemberProfile> GetMemberProfileByAccountId(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}