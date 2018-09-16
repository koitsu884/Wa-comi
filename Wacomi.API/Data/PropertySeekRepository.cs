using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class PropertySeekRepository : RepositoryBase, IPropertySeekRepository
    {
        public PropertySeekRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Property>> GetLatestProperties(GenderEnum? gender = null)
        {
            var query = _context.Properties.Include(p => p.MainPhoto)
                                .Include(p => p.AppUser).ThenInclude(a => a.MainPhoto)
                                .Include(p => p.City)
                                .Where(p => p.IsActive)
                                .OrderByDescending(p => p.DateUpdated)
                                .Take(12)
                                .AsQueryable();
            if(gender != null)
            {
                switch(gender){
                    case GenderEnum.MALE:
                        query = query.Where( p => p.Gender != GenderEnum.FEMALE);
                        break;
                    case GenderEnum.FEMALE:
                        query = query.Where( p => p.Gender != GenderEnum.MALE);
                        break;
                    default:
                        query = query.Where( p => p.Gender == GenderEnum.SECRET);
                        break;
                }
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesByUser(int userId)
        {
            return await _context.Properties
                                .Include(p => p.MainPhoto)
                                .Include(p => p.City)
                                .Where(p => p.AppUserId == userId)
                                .OrderByDescending(p => p.DateUpdated)
                                .ToListAsync();
        }

        public async Task<PagedList<Property>> GetProperties(PaginationParams paginationParams, PropertySeekParameters searchOptions)
        {
            var query = _context.Properties.Include(p => p.MainPhoto)
                                .Include(p => p.Categorizations).ThenInclude(c => c.PropertySeekCategory)
                                .Include(p => p.AppUser).ThenInclude(au => au.MainPhoto)
                                .Include(p => p.City)
                                .Where(p => p.IsActive)
                                .OrderByDescending(p => p.DateUpdated)
                                .AsQueryable();

            if (searchOptions.CategoryIds != null && searchOptions.CategoryIds.Length > 0)
            {
                query = query.Where(a => a.Categorizations.Any(c => searchOptions.CategoryIds.Contains(c.PropertySeekCategoryId)));
            }

            if (searchOptions.RentTypes != null && searchOptions.RentTypes.Length > 0)
            {
                query = query.Where(a => searchOptions.RentTypes.Contains(a.RentType));
            }

            if (searchOptions.rentMin != null)
                query = query.Where(a => a.Rent >= searchOptions.rentMin);

            if (searchOptions.rentMax != null)
                query = query.Where(a => a.Rent <= searchOptions.rentMax);

            if (searchOptions.Internet > 0)
                query = query.Where(a => a.Internet >= searchOptions.Internet);

            if (searchOptions.DateFrom != null)
                query = query.Where(a => a.DateAvailable == null || a.DateAvailable <= searchOptions.DateFrom);

            if (searchOptions.Pet == PropertyRequestEnum.Yes)
                query = query.Where(a => a.HasPet);
            else if (searchOptions.Pet == PropertyRequestEnum.No)
                query = query.Where(a => !a.HasPet);

            if (searchOptions.Child == PropertyRequestEnum.Yes)
                query = query.Where(a => a.HasChild);
            else if (searchOptions.Child == PropertyRequestEnum.No)
                query = query.Where(a => !a.HasChild);


            if (searchOptions.Area_top != null)
            {
                query = query.Where(a => searchOptions.Area_bottom < a.Latitude && a.Latitude < searchOptions.Area_top);
                query = query.Where(a => searchOptions.Area_right < a.Longitude && a.Longitude < searchOptions.Area_left);
            }
            else if (searchOptions.CityId > 0)
            {
                query = query.Where(a => a.CityId == searchOptions.CityId);
            }

            if (searchOptions.Gender == GenderEnum.MALE)
                query = query.Where(a => a.Gender != GenderEnum.FEMALE);
            else if (searchOptions.Gender == GenderEnum.FEMALE)
                query = query.Where(a => a.Gender != GenderEnum.MALE);
            else
                query = query.Where(a => a.Gender == 0);


            return await PagedList<Property>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<Property> GetProperty(int id)
        {
            return await _context.Properties.Include(p => p.Categorizations)
                                             .ThenInclude(c => c.PropertySeekCategory)
                                             .Include(p => p.AppUser)
                                             .Include(p => p.Photos)
                                             .Include(p => p.City)
                                             .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<PropertySeekCategory>> GetPropertyCategories()
        {
            return await _context.PropertySeekCategories.ToListAsync();
        }

        public async Task<bool> PropertyExists(int id)
        {
            return await _context.Properties.AnyAsync(p => p.Id == id);
        }
    }
}