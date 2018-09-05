using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IAttractionRepository : IRepositoryBase
    {
        Task<Attraction> GetAttraction(int id);
        Task<bool> AttractionExists(int id);
        Task<IEnumerable<Attraction>> GetAttractions(int[] categoryIds, int cityId = 0, int appUserId = 0);
        Task<IEnumerable<Attraction>> GetLatestAttractions();
        Task<IEnumerable<AttractionCategory>> GetAttractionCategories();
        Task<AttractionReview> GetAttractionReview(int id);
        Task<IEnumerable<AttractionReview>> GetLatestAttractionReviews();
        Task<IEnumerable<AttractionReview>> GetAttractionReviewsFor(int attractionId);
    }
}