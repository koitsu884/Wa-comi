using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.Xunit.MockRepositories
{
    public class CircleRepoFake : RepositoryBase, ICircleRepository
    {
        private readonly List<Circle> _circleList;
        private readonly List<AppUser> _appUsers;
        private readonly List<CircleMember> _circleMemberList;
        private readonly List<CircleCategory> _circleCategories;

        public CircleRepoFake(ApplicationDbContext context) : base(context)
        {
            _appUsers = CommonTestData.AppUserList;

            _circleCategories = new List<CircleCategory>(){
                new CircleCategory(){ Id = 1, Name="カテゴリー１"},
                new CircleCategory(){ Id = 2, Name="カテゴリー2"},
                new CircleCategory(){ Id = 3, Name="カテゴリー3"},
            };

            _circleList = new List<Circle>(){
                new Circle(){
                    Id = 1, 
                    Name="サークル１", 
                    Category = _circleCategories[0], 
                    CategoryId = _circleCategories[0].Id, 
                    AppUser = _appUsers[0],
                    AppUserId = _appUsers[0].Id
                    },
                new Circle(){
                    Id = 2, 
                    Name="サークル2", 
                    Category = _circleCategories[1], 
                    CategoryId = _circleCategories[1].Id, 
                    AppUser = _appUsers[1],
                    AppUserId = _appUsers[1].Id
                    },
                new Circle(){
                    Id = 3, 
                    Name="サークル3", 
                    Category = _circleCategories[1], 
                    CategoryId = _circleCategories[1].Id, 
                    AppUser = _appUsers[2],
                    AppUserId = _appUsers[2].Id
                    }
            };

            _circleMemberList = new List<CircleMember>(){
                new CircleMember{AppUserId = 1, CircleId = 1, Role = CircleRoleEnum.OWNER},
                new CircleMember{AppUserId = 2, CircleId = 1, Role = CircleRoleEnum.MEMBER},
                new CircleMember{AppUserId = 3, CircleId = 2, Role = CircleRoleEnum.OWNER},
            };
        }

        public Task ApproveAll(int circleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> CheckUpdatePermission(int userId, int circleId)
        {
            return Task.FromResult(_circleMemberList.Any(cm => cm.AppUserId == userId && cm.CircleId == circleId && cm.Role == CircleRoleEnum.OWNER));
        }

        public Task<bool> CircleExists(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Circle> GetCircle(int id)
        {
            return Task.FromResult(_circleList.FirstOrDefault(c => c.Id == id));
        }

        public Task<IEnumerable<CircleCategory>> GetCircleCategories()
        {
            return Task.FromResult((IEnumerable<CircleCategory>)this._circleCategories);
        }

        public Task<int> GetCircleCountForUser(int userId)
        {
            return Task.FromResult(_circleList.Count(c => c.AppUserId == userId));
        }

        public Task<CircleMember> GetCircleMember(int appUserId, int circleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> GetCircleMemberCount(int circleId)
        {
            return Task.FromResult(_circleMemberList.Count(ml => ml.CircleId == circleId));
        }

        public Task<PagedList<CircleMember>> GetCircleMemberList(PaginationParams paginationParams, int circleId, CircleRoleEnum? role = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<CircleRequest> GetCircleRequest(int appUserId, int circleId)
        {
            return Task.FromResult<CircleRequest>(null);
        }

        public Task<PagedList<Circle>> GetCircles(PaginationParams paginationParams, CircleSearchParameter searchOptions)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Circle>> GetCirclesByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CircleTopic> GetCircleTopic(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<CircleTopicComment>> GetCircleTopicCommentList(PaginationParams paginationParams, int circleTopicId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<CircleTopicCommentReply>> GetCircleTopicCommentReplies(int circleTopicCommentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<CircleTopic>> GetCircleTopicList(PaginationParams paginationParams, int circleId)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<T> GetDataRecordByTableName<T>(string tableName) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<CircleMember>> GetLatestCircleMemberList(int circleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Circle>> GetLatestCircles()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<CircleRequest>> GetRequestsForCircle(int circleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsMember(int appUserId, int circleId)
        {
            return Task.FromResult(_circleMemberList.Any(cm => cm.AppUserId == appUserId && cm.CircleId == circleId));
        }

        public Task<bool> IsOwner(int appUserId, int circleId)
        {
            return Task.FromResult(_circleMemberList.Any(cm => cm.AppUserId == appUserId && cm.CircleId == circleId && cm.Role == CircleRoleEnum.OWNER));
        }

        public Task<bool> RecordExist(string recordType, int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> RequestSent(int appUserId, int circleId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SaveAll()
        {
            throw new System.NotImplementedException();
        }

        public void Test()
        {
            throw new System.NotImplementedException();
        }
    }
}