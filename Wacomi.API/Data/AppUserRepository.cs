using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class AppUserRepository : RepositoryBase, IAppUserRepository
    {
        public AppUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<AppUser> GetAppUser(int id)
        {
            return await _context.AppUsers.Include(au => au.City)
                                          .Include(au => au.Photos)
                                          .FirstOrDefaultAsync(au => au.Id == id);
        }


        public async Task<AppUser> GetAppUserByAccountId(string accountId)
        {
            return await _context.AppUsers.Include(au => au.City)
                                          .Include(au => au.Photos)
                                         //   .Include(au => au.Blogs)
                                         .FirstOrDefaultAsync(au => au.AccountId == accountId);
        }

        public async Task AddLikeCountToUser(int userId)
        {
            var appUser = await GetAppUser(userId);
            if (appUser != null)
                appUser.TotalLike++;
            //TODO: add for month, week
        }

        public async Task<BusinessProfile> GetBusinessProfile(int id)
        {
            return await _context.BusinessProfiles.Include(b => b.AppUser)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<MemberProfile> GetMemberProfile(int id)
        {
            return await _context.MemberProfiles.Include(m => m.HomeTown)
                                                .Include(m => m.AppUser)
                                                // .Include(m => m.AppUser.Photos)
                                                // .Include(m => m.AppUser.Blogs)
                                                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MemberProfile> GetMemberProfileByAccountId(string id)
        {
            return await _context.MemberProfiles.Include(m => m.HomeTown)
                                                .Include(m => m.AppUser)
                                                .FirstOrDefaultAsync(m => m.AppUser.AccountId == id);
        }
    }
}