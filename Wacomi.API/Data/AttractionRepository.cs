using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class AttractionRepository : RepositoryBase, IAttractionRepository
    {
        public AttractionRepository(ApplicationDbContext context) : base(context){}

        public async Task<Attraction> GetAttraction(int id)
        {
           return await _context.Attractions.Include(a => a.Categorizations)
                                            .ThenInclude(c => c.AttractionCategory)
                                            .Include(a => a.AppUser)
                                            .Include(a => a.Photos)
                                            .Include(a => a.City)
                                            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Attraction>> GetAttractions(int[] categoryIds, int cityId = 0, int appUserId = 0)
        {
           var query = _context.Attractions.Include(a => a.Categorizations)
                                            .ThenInclude(c => c.AttractionCategory)
                                            .Include(a => a.AppUser)
                                            .Include(a => a.Photos)
                                            .Include(a => a.City)
                                            .AsQueryable();

            if(categoryIds != null && categoryIds.Length > 0){
                query = query.Where(a => a.Categorizations.Any(c => categoryIds.Contains(c.AttractionCategoryId )));
            }

            if(cityId > 0){
                query = query.Where(a => a.CityId == cityId);
            }

            if(appUserId > 0){
                query = query.Where(a => a.AppUserId == appUserId);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Attraction>> GetLatestAttractions()
        {
           return await _context.Attractions.Include(a => a.Categorizations)
                                            .ThenInclude(c => c.AttractionCategory)
                                            .Include(a => a.AppUser)
                                            .Include(a => a.MainPhoto)
                                            .Include(a => a.City)
                                            .OrderByDescending(a => a.DateCreated)
                                            .Take(20)
                                            .ToListAsync();

        }

        public async Task<IEnumerable<AttractionCategory>> GetAttractionCategories(){
            return await _context.AttractionCategories.ToListAsync();
        }
    }
}