using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class CircleSearchParameter{
        public int? CategoryId {get; set;}
        public int? CityId {get; set;}
    }

    public interface ICircleRepository : IRepositoryBase
    {
        void Test();
        Task<Circle> GetCircle(int id);
        Task<bool> CircleExists(int id);
        Task<bool> CheckUpdatePermission(int userId, int circleId);
        Task<PagedList<Circle>> GetCircles(PaginationParams paginationParams, CircleSearchParameter searchOptions);
        Task<IEnumerable<Circle>> GetLatestCircles();
        Task<CircleMember> GetCircleMember(int appUserId, int circleId);
        Task<int> GetCircleMemberCount(int circleId);
        Task<PagedList<CircleMember>> GetCircleMemberList(PaginationParams paginationParams, int circleId, CircleRoleEnum? role = null);
         Task<IEnumerable<CircleMember>> GetLatestCircleMemberList(int circleId);
        Task<IEnumerable<Circle>> GetCirclesByUser(int userId); //Should get circle topic made by same user as well
        Task<int> GetCircleCountForUser( int userId);
        Task<IEnumerable<CircleCategory>> GetCircleCategories();
        // Task<IEnumerable<CircleTopic>> GetLatestCircleTopic(int circleId); //No need..?
        Task<CircleTopic> GetCircleTopic(int id);
        Task<PagedList<CircleTopic>> GetCircleTopicList(PaginationParams paginationParams, int circleId);

        Task<PagedList<CircleTopicComment>> GetCircleTopicCommentList(PaginationParams paginationParams, int circleTopicId);
        Task<IEnumerable<CircleTopicCommentReply>> GetCircleTopicCommentReplies(int circleTopicCommentId);

        //Circle Member functions
        Task<bool> IsMember(int appUserId, int circleId);
        Task<bool> IsOwner(int appUserId, int circleId);
        Task<CircleRequest> GetCircleRequest(int appUserId, int circleId);
        Task<bool> RequestSent(int appUserId, int circleId);
        Task ApproveAll(int circleId);
        Task<IEnumerable<CircleRequest>> GetRequestsForCircle(int circleId);
        // Task<

    }
}