using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class ClanSeekRepository : RepositoryBase, IClanSeekRepository
    {
        public ClanSeekRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ClanSeek> GetClanSeek(int id)
        {
            return await _context.ClanSeeks.Include(cs => cs.Category)
                                           .Include(cs => cs.AppUser)
                                           .Include(cs => cs.Location)
                                           .Include(cs => cs.MainPhoto)
                                           .Include(cs => cs.Photos)
                                           .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<ClanSeekCategory>> GetClanSeekCategories()
        {
            return await _context.ClanSeekCategories.ToListAsync();
        }

        //public async Task<PagedList<ClanSeek>> GetClanSeeks(int? categoryId = null, int? cityId = null, bool? latest = null)
        public async Task<PagedList<ClanSeek>> GetClanSeeks(PaginationParams paginationParams, int? categoryId = null, int? cityId = null)
        {
            var clanSeeks = _context.ClanSeeks.Include(cs => cs.Category)
                                          .Include(cs => cs.AppUser)
                                            .ThenInclude(u => u.MainPhoto)
                                          .Include(cs => cs.MainPhoto)
                                          .Include(cs => cs.Location)
                                          .OrderByDescending(cs => cs.LastActive)
                                          .AsQueryable();
            if (categoryId != null)
            {
                clanSeeks = clanSeeks.Where(cs => cs.CategoryId == categoryId);
            }

            if (cityId != null)
            {
                clanSeeks = clanSeeks.Where(cs => cs.LocationId == cityId);
            }
            clanSeeks = clanSeeks.Where(cs => cs.IsActive == true);

            return await PagedList<ClanSeek>.CreateAsync(clanSeeks, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<ClanSeek>> GetClanSeeksByUser(int userId)
        {
            return await _context.ClanSeeks.Include(cs => cs.Category)
                                           .Include(cs => cs.Location)
                                           .Include(cs => cs.MainPhoto)
                                           .Where(cs => cs.AppUserId == userId)
                                           .OrderByDescending(cs => cs.LastActive).ToListAsync();
        }

        public async Task<int> GetClanSeeksCountByUser(int userId)
        {
            return await _context.ClanSeeks.Where(cs => cs.AppUserId == userId).CountAsync();
        }
    }
}