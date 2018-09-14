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
        public AttractionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Attraction> GetAttraction(int id)
        {
            return await _context.Attractions.Include(a => a.Categorizations)
                                             .ThenInclude(c => c.AttractionCategory)
                                             .Include(a => a.AppUser)
                                             .Include(a => a.Photos)
                                             .Include(a => a.AttractionLikes)
                                             .Include(a => a.AttractionReviews).ThenInclude(ar => ar.AppUser).ThenInclude(au => au.MainPhoto)
                                             .Include(a => a.City)
                                             .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> AttractionExists(int id)
        {
            return await _context.Attractions.AnyAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Attraction>> GetAttractions(int[] categoryIds, int cityId = 0, int appUserId = 0)
        {
            var query = _context.Attractions.Include(a => a.Categorizations)
                                             .ThenInclude(c => c.AttractionCategory)
                                             .Include(a => a.AppUser)
                                             .Include(a => a.Photos)
                                             .Include(a => a.AttractionReviews).ThenInclude(ar => ar.AppUser).ThenInclude(au => au.MainPhoto)
                                             .Include(a => a.City)
                                             .AsQueryable();

            if (categoryIds != null && categoryIds.Length > 0)
            {
                query = query.Where(a => a.Categorizations.Any(c => categoryIds.Contains(c.AttractionCategoryId)));
            }

            if (cityId > 0)
            {
                query = query.Where(a => a.CityId == cityId);
            }

            if (appUserId > 0)
            {
                query = query.Where(a => a.AppUserId == appUserId);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Attraction>> GetAttractionsByUser(int userId)
        {
            return await _context.Attractions.Include(a => a.Categorizations)
                                             .ThenInclude(c => c.AttractionCategory)
                                             .Include(a => a.MainPhoto)
                                             .Include(a => a.City)
                                             .Include(a => a.AttractionReviews).ThenInclude(ar => ar.AppUser).ThenInclude(au => au.MainPhoto)
                                             .OrderByDescending(a => a.DateCreated)
                                             .Where(a => a.AppUserId == userId)
                                             .ToListAsync();
        }

        public async Task<IEnumerable<Attraction>> GetLatestAttractions()
        {
            return await _context.Attractions.Include(a => a.Categorizations)
                                             .ThenInclude(c => c.AttractionCategory)
                                             .Include(a => a.AppUser)
                                             .Include(a => a.MainPhoto)
                                             .Include(a => a.City)
                                             .Include(a => a.AttractionReviews).ThenInclude(ar => ar.AppUser).ThenInclude(au => au.MainPhoto)
                                             .OrderByDescending(a => a.DateCreated)
                                             .Take(12)
                                             .ToListAsync();

        }

        public async Task<double> GetAttractionRateAverage(int attractionId)
        {
            return await _context.AttractionReviews.Where(ar => ar.AttractionId == attractionId && ar.Score > 0)
                                .AverageAsync(ar => ar.Score);

        }

        public async Task<IEnumerable<AttractionCategory>> GetAttractionCategories()
        {
            return await _context.AttractionCategories.ToListAsync();
        }

        public async Task<AttractionReview> GetAttractionReview(int id)
        {
            return await _context.AttractionReviews.Include(a => a.AppUser).ThenInclude(au => au.MainPhoto)
                                             .Include(a => a.Photos)
                                             .Include(a => a.MainPhoto)
                                             .Include(a => a.AttractionReviewLikes)
                                             .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AttractionReview> GetAttractionReviewByUser(int userId, int attractionId)
        {
            return await _context.AttractionReviews.FirstOrDefaultAsync(a => a.AppUserId == userId && a.AttractionId == attractionId);
        }

        public async Task<IEnumerable<AttractionReview>> GetLatestAttractionReviews()
        {
            return await _context.AttractionReviews.Include(a => a.AppUser).ThenInclude(au => au.MainPhoto)
                                            .Include(a => a.MainPhoto)
                                            .Include(a => a.Attraction).ThenInclude(a => a.City)
                                            .Include(a => a.Attraction).ThenInclude(a => a.MainPhoto)
                                            .OrderByDescending(r => r.DateCreated)
                                            .Take(12)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<AttractionReview>> GetAttractionReviewsByUser(int userId)
        {
            return await _context.AttractionReviews.Include(a => a.MainPhoto)
                                            .Include(a => a.Attraction).ThenInclude(a => a.City)
                                            .Include(a => a.Attraction).ThenInclude(a => a.MainPhoto)
                                            .OrderByDescending(r => r.DateCreated)
                                            .Where(ar => ar.AppUserId == userId)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<AttractionReview>> GetAttractionReviewsFor(int attractionId)
        {
            return await _context.AttractionReviews.Include(a => a.AppUser).ThenInclude(au => au.MainPhoto)
                                            .Include(a => a.Photos)
                                            .Include(a => a.MainPhoto)
                                            .Include(ar => ar.AttractionReviewLikes)
                                            .Where(a => a.AttractionId == attractionId)
                                            .ToListAsync();
        }

        public async Task<List<Photo>> GetAllReviewPhotosForAttraction(int attractionId)
        {
            var attractionReviews = await _context.AttractionReviews.Include(ar => ar.Photos)
                                                                    .Where(ar => ar.AttractionId == attractionId && ar.Photos.Count() > 0)
                                                                    .ToListAsync();
            List<Photo> photos = new List<Photo>();
            foreach (var review in attractionReviews)
            {
                photos.AddRange(review.Photos);
            }
            return photos;
        }

        public async void DeleteReviewsForAttraction(int attractionId)
        {
            var deletingReviews = await _context.AttractionReviews.Include(ar => ar.Photos).Where(ar => ar.AttractionId == attractionId).ToListAsync();
        }

        public async Task<bool> AttractionLiked(int appUserId, int attractionId)
        {
            return await _context.AttractionLikes.AnyAsync(al => al.AppUserId == appUserId && al.AttractionId == attractionId);
        }
        public async Task<AttractionLike> GetAttractionLike(int appUserId, int attractionId)
        {
            return await _context.AttractionLikes.FirstOrDefaultAsync(al => al.AppUserId == appUserId && al.AttractionId == attractionId);
        }

        public async Task<bool> AttractionReviewLiked(int appUserId, int reviewId)
        {
            return await _context.AttractionReviewLikes.AnyAsync(al => al.AppUserId == appUserId && al.AttractionReviewId == reviewId);
        }
        public async Task<AttractionReviewLike> GetAttractionReviewLike(int appUserId, int reviewId)
        {
            return await _context.AttractionReviewLikes.FirstOrDefaultAsync(al => al.AppUserId == appUserId && al.AttractionReviewId == reviewId);
        }

        public async Task<bool> AttractionReviewed(int appUserId, int attractionId)
        {
            return await _context.AttractionReviews.AnyAsync(ar => ar.AppUserId == appUserId && ar.AttractionId == attractionId);
        }

    }
}