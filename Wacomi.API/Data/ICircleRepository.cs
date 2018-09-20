using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class CircleSearchParameter{
        public int CategoryId {get; set;}
        public int CityId {get; set;}
    }

    public interface ICircleRepository : IRepositoryBase
    {
        void Test();
        Task<Circle> GetCircle(int id);
        Task<bool> CircleExists(int id);
        Task<bool> CheckUpdatePermission(int userId);
        Task<PagedList<Circle>> GetCircles(PaginationParams paginationParams, CircleSearchParameter searchOptions);
        Task<IEnumerable<Circle>> GetLatestCircles();
        Task<IEnumerable<AppUser>> GetCircleMemberList(int circleId, CircleRoleEnum? role = null);
        Task<IEnumerable<Circle>> GetCirclesByUser(int userId); //Should get circle topic made by same user as well
        Task<int> GetCircleCountForUser( int userId);
        Task<IEnumerable<CircleCategory>> GetCircleCategories();
        // Task<IEnumerable<CircleTopic>> GetLatestCircleTopic(int circleId); //No need..?
        Task<CircleTopic> GetCircleTopic(int id);
        Task<PagedList<CircleTopic>> GetCircleTopicList(PaginationParams paginationParams, int circleId);

        Task<PagedList<CircleTopicComment>> GetCircleTopicCommentList(PaginationParams paginationParams, int circleTopicId);
        Task<IEnumerable<CircleTopicCommentReply>> GetCircleTopicCommentReplies(int circleTopicCommentId);

    }
}