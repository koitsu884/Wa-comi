using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class PhotoRepository : RepositoryBase, IPhotoRepository
    {
        public PhotoRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Photo> GetPhotoByPublicId(string publicId)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<IEnumerable<Photo>> GetPhotosForRecord(string recordType, int recordId)
        {
            switch (recordType.ToLower())
            {
                case "appuser":
                    var appUser = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (appUser != null)
                    {
                        return appUser.Photos.ToList();
                    }
                    break;
                case "attraction":
                    var attraction = await _context.Attractions.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (attraction != null)
                    {
                        return attraction.Photos.ToList();
                    }
                    break;
                 case "attractionreview":
                    var attractionReview = await _context.AttractionReviews.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (attractionReview != null)
                    {
                        return attractionReview.Photos.ToList();
                    }
                    break;
                case "clanseek":
                    var clanSeek = await _context.ClanSeeks.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (clanSeek != null)
                    {
                        return clanSeek.Photos.ToList();
                    }
                    break;
                case "property":
                    var properties = await _context.Properties.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (properties != null)
                    {
                        return properties.Photos.ToList();
                    }
                    break;
            }
            return null;
        }
        public async Task<IEnumerable<Photo>> GetPhotosForAppUser(int id)
        {
            var appUser = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
            if (appUser != null)
            {
                return appUser.Photos.ToList();
            }
            return null;
        }
    }
}