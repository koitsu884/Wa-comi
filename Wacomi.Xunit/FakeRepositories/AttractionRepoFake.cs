using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Data;
using Wacomi.API.Models;
using Wacomi.Xunit.MockRepositories;

namespace Wacomi.Xunit.FakeRepositories
{
    public class AttractionRepoFake : RepositoryBase, IAttractionRepository
    {
        private readonly List<Attraction> _attractionlist;
        private readonly List<AttractionReview> _attractionReviewList;
        private readonly List<AttractionLike> _attractionLikeList;
        public AttractionRepoFake(ApplicationDbContext context) : base(context)
        {
            _attractionlist = new List<Attraction>(){
            new Attraction(){
                Id =1,
                Name = "Attraction 1",
                Introduction = "アトラクション 1",
                AppUser = CommonTestData.AppUserList[0],
                AppUserId = CommonTestData.AppUserList[0].Id,
                CityId = 1,
                Categorizations = new List<AttractionCategorization>(){
                    new AttractionCategorization(){AttractionId = 1, AttractionCategoryId = 1},
                    new AttractionCategorization(){AttractionId = 1, AttractionCategoryId = 2},
                    new AttractionCategorization(){AttractionId = 1, AttractionCategoryId = 3}
                }
            },
           new Attraction(){
                Id =2,
                Name = "Attraction 2",
                Introduction = "アトラクション 2",
                AppUser = CommonTestData.AppUserList[1],
                AppUserId = CommonTestData.AppUserList[1].Id,
                CityId = 1,
                Categorizations = new List<AttractionCategorization>(){
                    new AttractionCategorization(){AttractionId = 2, AttractionCategoryId = 3}
                }
            },
            new Attraction(){
                Id =1,
                Name = "Attraction 3",
                Introduction = "アトラクション 3",
                AppUser = CommonTestData.AppUserList[2],
                AppUserId = CommonTestData.AppUserList[2].Id,
                CityId = 1,
                Categorizations = new List<AttractionCategorization>()
            },
        };

            _attractionReviewList = new List<AttractionReview>(){
            new AttractionReview(){
                AttractionId =1,
                AppUserId = CommonTestData.AppUserList[0].Id,
                Score = 5,
                Review = "Good!!"
            },
            new AttractionReview(){
                AttractionId =1,
                AppUserId = CommonTestData.AppUserList[1].Id,
                Score = 3,
                Review = "Not bad"
            },
            new AttractionReview(){
                AttractionId = 3,
                AppUserId = CommonTestData.AppUserList[1].Id,
                Score = 1,
                Review = "Poor"
            },
        };

            _attractionLikeList = new List<AttractionLike>(){
            new AttractionLike(){
                AttractionId =1,
                AppUserId = CommonTestData.AppUserList[0].Id,
            },
           new AttractionLike(){
                AttractionId =1,
                AppUserId = CommonTestData.AppUserList[1].Id,
            },
            new AttractionLike(){
                AttractionId =2,
                AppUserId = CommonTestData.AppUserList[2].Id,
            },
        };
        }

        public Task<bool> AttractionExists(int id)
        {
            return Task.FromResult(_attractionlist.Any(a => a.Id == id));
        }

        public Task<bool> AttractionLiked(int appUserId, int attractionId)
        {
            return Task.FromResult(_attractionLikeList.Any(al => al.AppUserId == appUserId && al.AttractionId == attractionId));
        }

        public Task<bool> AttractionReviewed(int appUserId, int attractionId)
        {
            return Task.FromResult(_attractionReviewList.Any(ar => ar.AttractionId == attractionId && ar.AppUserId == appUserId));
        }

        public Task<bool> AttractionReviewLiked(int appUserId, int reviewId)
        {
            return Task.FromResult(false);
        }

        public void DeleteReviewsForAttraction(int attractionId)
        {
            return;
        }

        public Task<List<Photo>> GetAllReviewPhotosForAttraction(int attractionId)
        {
            return Task.FromResult<List<Photo>>(null);
        }

        public Task<Attraction> GetAttraction(int id)
        {
            return Task.FromResult(_attractionlist.FirstOrDefault(a => a.Id == id));
        }

        public Task<IEnumerable<AttractionCategory>> GetAttractionCategories()
        {
            return Task.FromResult<IEnumerable<AttractionCategory>>(null);
        }

        public Task<AttractionLike> GetAttractionLike(int appUserId, int attractionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<double> GetAttractionRateAverage(int attractionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<AttractionReview> GetAttractionReview(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<AttractionReview> GetAttractionReviewByUser(int userId, int attractionId)
        {
            return Task.FromResult(_attractionReviewList.FirstOrDefault(ar => ar.AppUserId == userId && ar.AttractionId == attractionId));
        }

        public Task<AttractionReviewLike> GetAttractionReviewLike(int appUserId, int reviewId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AttractionReview>> GetAttractionReviewsByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AttractionReview>> GetAttractionReviewsFor(int attractionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Attraction>> GetAttractions(int[] categoryIds, int cityId = 0, int appUserId = 0)
        {
            var query = _attractionlist.AsQueryable();
            if(cityId > 0)
                query.Where(a => a.CityId == cityId);
            if(categoryIds != null && categoryIds.Length > 0){
                query = query.Where(a => a.Categorizations.Any(c => categoryIds.Contains(c.AttractionCategoryId)));
            }
            return Task.FromResult(query.ToList() as IEnumerable<Attraction>);
        }

        public Task<IEnumerable<Attraction>> GetAttractionsByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<AttractionReview>> GetLatestAttractionReviews()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Attraction>> GetLatestAttractions()
        {
            throw new System.NotImplementedException();
        }
    }
}