using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.Xunit.MockRepositories
{
    public class CircleRepoFake : ICircleRepository
    {
        private readonly List<Circle> _circleList;
        private readonly List<AppUser> _appUsers;
        private readonly List<CircleMember> _circleMemberList;
        private readonly List<CircleCategory> _circleCategories;
        private readonly List<CircleRequest> _circleRequestList;
        private readonly List<CircleEvent> _circleEventList;
        private readonly List<CircleEventParticipation> _circleParticipantsList;

        public CircleRepoFake(ApplicationDbContext context)
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

            _circleRequestList = new List<CircleRequest>(){
                new CircleRequest{AppUserId = 3, CircleId =1},
                new CircleRequest{AppUserId = 1, CircleId =2}
            };

            _circleEventList = new List<CircleEvent>(){
                new CircleEvent{
                    Id = 1,
                    AppUserId = _appUsers[0].Id, 
                    CircleId = 1,
                    Title = "サークル１　イベント",
                    Description = "サークル１　イベント テスト"},
                new CircleEvent{
                    Id = 2,
                    AppUserId = _appUsers[0].Id, 
                    CircleId = 1,
                    Title = "サークル１　イベント 2",
                    Description = "サークル１　イベント テスト 制限有",
                    MaxNumber = 2
                    },
                new CircleEvent{
                    Id = 3,
                    AppUserId = _appUsers[0].Id, 
                    CircleId = 1,
                    Title = "サークル１　イベント 3",
                    Description = "サークル１　イベント テスト 承認制",
                    ApprovalRequired = true
                    },
            };

            _circleParticipantsList = new List<CircleEventParticipation>();
        }

        public void Add<T>(T entity) where T : class
        {
            var entityType = entity.GetType();
            if(entityType == typeof(CircleMember))
                _circleMemberList.Add(entity as CircleMember);
            if(entityType == typeof(CircleEvent))
                _circleEventList.Add(entity as CircleEvent);
            if(entityType == typeof(CircleEventParticipation))
                _circleParticipantsList.Add(entity as CircleEventParticipation);
            return;
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

        public void Delete<T>(T entity) where T : class
        {
            var entityType = entity.GetType();
            if(entityType == typeof(CircleMember))
                _circleMemberList.Remove(entity as CircleMember);
            else if(entityType == typeof(CircleRequest))
                _circleRequestList.Remove(entity as CircleRequest);
            return;
        }

        public void DeleteAll<T>(T entities) where T : class
        {
            throw new System.NotImplementedException();
        }

        public Task<T> Get<T>(int recordId) where T : class
        {
            if(typeof(T) == typeof(Circle))
                return Task.FromResult(_circleList.Find(c => c.Id == recordId) as T);
            if(typeof(T) == typeof(CircleEvent))
                return Task.FromResult(_circleEventList.Find(c => c.Id == recordId) as T);
            return Task.FromResult(null as T);
        }

        public Task<List<Photo>> GetAllCommentPhotosForCircleTopic(int circleTopicId)
        {
            throw new NotImplementedException();
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

        public Task<CircleEventParticipation> GetCircleEventParticipation(int appUserId, int eventId)
        {
            return Task.FromResult(_circleParticipantsList.Find(cpl => cpl.AppUserId == appUserId && cpl.CircleEventId == eventId));
        }

        public Task<int> GetCircleEventParticipationCount(int eventId)
        {
            return Task.FromResult(_circleParticipantsList.Where(cpl => cpl.CircleEventId == eventId).Count());
        }

        public Task<PagedList<CircleEventParticipation>> GetCircleEventParticipationList(PaginationParams paginationParams, int eventId, CircleEventParticipationStatus? status = null)
        {
            throw new NotImplementedException();
        }

        public Task<CircleMember> GetCircleMember(int appUserId, int circleId)
        {
            return Task.FromResult(_circleMemberList.FirstOrDefault(cm => cm.AppUserId == appUserId && cm.CircleId == circleId));
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
            return Task.FromResult<CircleRequest>(_circleRequestList.FirstOrDefault(cr => cr.AppUserId == appUserId && cr.CircleId == circleId));
        }

        public Task<PagedList<Circle>> GetCircles(PaginationParams paginationParams, CircleSearchParameter searchOptions)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Circle>> GetCirclesByUser(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<Circle>> GetCirclesByUser(PaginationParams paginationParams, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<Circle>> GetCirclesOwnedByUser(PaginationParams paginationParams, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<CircleTopic> GetCircleTopic(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<CircleTopic>> GetCircleTopicByUser(PaginationParams paginationParams, int circleId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<CircleTopicComment> GetCircleTopicComment(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCircleTopicCommentCount(int circleTopicId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<CircleTopicComment>> GetCircleTopicCommentList(PaginationParams paginationParams, int circleTopicId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<CircleTopicCommentReply>> GetCircleTopicCommentReplies(int circleTopicCommentId)
        {
            throw new System.NotImplementedException();
        }

        public Task<CircleTopicCommentReply> GetCircleTopicCommentReply(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCircleTopicCommentReplyCount(int circleTopicCommentId)
        {
            throw new NotImplementedException();
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

        public Task<IEnumerable<CircleTopic>> GetLatestCircleTopicList(int circleId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CircleRequest>> GetRequestsForCircle(int circleId)
        {
            return Task.FromResult((IEnumerable<CircleRequest>)_circleRequestList.Where(cr => cr.CircleId == circleId).ToList());
        }

        public async Task<bool> IsEventFull(int eventId)
        {
            var temp = _circleEventList.FirstOrDefault(ce => ce.Id == eventId);
            return temp != null && temp.MaxNumber != null ? await GetCircleEventParticipationCount(temp.Id) >= temp.MaxNumber : false;   
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
            return Task.FromResult(_circleRequestList.Any(cr => cr.AppUserId == appUserId && cr.CircleId == circleId));
        }

        public Task<int> SaveAll()
        {
            return Task.FromResult(1);
        }

        public void Test()
        {
            throw new System.NotImplementedException();
        }
    }
}