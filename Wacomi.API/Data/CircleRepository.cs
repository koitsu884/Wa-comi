using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;
using Wacomi.API.Models.Circles;

namespace Wacomi.API.Data
{
    public class CircleRepository : RepositoryBase, ICircleRepository
    {
        public CircleRepository(ApplicationDbContext context) : base(context) { }

        public void Test()
        {
            Dictionary<string, string> typeNameMap = new Dictionary<string, string>();
            var dbSetList = _context.GetType().GetProperties();
            foreach (var dbSet in dbSetList)
            {
                typeNameMap.Add(dbSet.GetType().ToString(), dbSet.Name);
            }
        }

        public async Task<bool> CircleExists(int id)
        {
            return await _context.Circles.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CheckUpdatePermission(int userId, int circleId)
        {
            return await _context.CircleMembers.AnyAsync(cm => cm.AppUserId == userId && cm.CircleId == circleId && cm.Role == CircleRoleEnum.OWNER);
        }
        public async Task<Circle> GetCircle(int id)
        {
            return await _context.Circles.Include(c => c.Category)
                                         .Include(c => c.City)
                                         .Include(c => c.AppUser).ThenInclude(a => a.MainPhoto)
                                         .Include(c => c.Photos)
                                         .Include(c => c.MainPhoto)
                                         .Include(c => c.CircleMemberList)
                                        .Include(c => c.Topics)
                                        .Where(c => c.Id == id)
                                        .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CircleCategory>> GetCircleCategories()
        {
            return await _context.CircleCategories.ToListAsync();
        }

        public async Task<int> GetCircleCountForUser(int userId)
        {
            return await _context.CircleMembers.CountAsync(cm => cm.AppUserId == userId && cm.Role == CircleRoleEnum.OWNER);
        }

        public async Task<PagedList<Circle>> GetCircles(PaginationParams paginationParams, CircleSearchParameter searchOptions)
        {
            var query = _context.Circles.Include(c => c.Category)
                                         .Include(c => c.City)
                                         .Include(c => c.MainPhoto)
                                        //  .Include(c => c.CircleMemberList)
                                        .AsQueryable();

            if (searchOptions.CityId != null && searchOptions.CityId > 0)
                query = query.Where(c => c.CityId == null || c.CityId == searchOptions.CityId);
            if (searchOptions.CategoryId != null && searchOptions.CategoryId > 0)
                query = query.Where(c => c.Category.Id == searchOptions.CategoryId);

            return await PagedList<Circle>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<CircleEvent>> GetCircleEvents(PaginationParams paginationParams, DateTime fromDate = default(DateTime), int circleId = 0, int circleCategoryId = 0, int cityId = 0, int appUserId = 0)
        {
            if (fromDate == default(DateTime))
                fromDate = DateTime.Now;
            // DateTime toDate = fromDate.AddMonths(1);
            var query = _context.CircleEvents.Include(ce => ce.AppUser).ThenInclude(a => a.MainPhoto)
                                .Include(ce => ce.Circle).ThenInclude(c => c.MainPhoto)
                                .Include(ce => ce.MainPhoto)
                                .Include(ce => ce.City)
                                .Include(ce => ce.CircleEventParticipations)
                                .Where(ce => ce.FromDate >= fromDate)
                                .OrderBy(ce => ce.FromDate)
                                .AsQueryable();

            if (circleId > 0)
                query = query.Where(ce => ce.CircleId == circleId);
            else
                query = query.Where(ce => ce.IsPublic == true);

            if (circleCategoryId > 0)
                query = query.Where(ce => ce.Circle.CategoryId == circleCategoryId);
            if (cityId > 0)
                query = query.Where(ce => ce.CityId == cityId);
            if (appUserId > 0)
                query = query.Where(ce => ce.AppUserId == appUserId);

            // return await query.ToListAsync();
            return await PagedList<CircleEvent>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }
        public async Task<CircleEventParticipation> GetCircleEventParticipation(int appUserId, int eventId)
        {
            return await _context.CircleEventParticipations.Include(cm => cm.AppUser).ThenInclude(a => a.MainPhoto).FirstOrDefaultAsync(cm => cm.AppUserId == appUserId && cm.CircleEventId == eventId);
        }

        public async Task<CircleEventParticipation> GetCircleEventFirstWaitingParticipation(int eventId)
        {
            return await _context.CircleEventParticipations.Include(cm => cm.AppUser)
                                                            .ThenInclude(a => a.MainPhoto)
                                                            .OrderBy(cm => cm.DateCreated)
                                                            .FirstOrDefaultAsync(cm => cm.CircleEventId == eventId && cm.Status == CircleEventParticipationStatus.Waiting);
        }

        public async Task<CircleEventComment> GetCircleEventComment(int id)
        {
            return await _context.CircleEventComments.Include(tc => tc.AppUser).ThenInclude(au => au.MainPhoto)
                                                     .FirstOrDefaultAsync(ctc => ctc.Id == id);
        }

        public async Task<int> GetCircleEventCommentCount(int circleEventId)
        {
            return await _context.CircleEventComments.Where(tc => tc.CircleEventId == circleEventId).CountAsync();
        }

        public async Task<IEnumerable<CircleEventCommentReply>> GetCircleEventCommentReplies(int circleTopicEventId)
        {
            return await _context.CircleEventCommentReplies.Include(cr => cr.AppUser).ThenInclude(a => a.MainPhoto)
                                                    .Where(tr => tr.CommentId == circleTopicEventId).OrderBy(tr => tr.DateCreated).ToListAsync();
        }
        public async Task<int> GetCircleEventCommentReplyCount(int circleEventCommentId)
        {
            return await _context.CircleEventCommentReplies.Where(tc => tc.CommentId == circleEventCommentId).CountAsync();
        }

        public async Task<CircleMember> GetCircleMember(int appUserId, int circleId)
        {
            return await _context.CircleMembers.Include(cm => cm.AppUser).ThenInclude(a => a.MainPhoto).FirstOrDefaultAsync(cm => cm.AppUserId == appUserId && cm.CircleId == circleId);
        }

        public async Task<int> GetCircleMemberCount(int circleId)
        {
            return await _context.CircleMembers.CountAsync(cm => cm.CircleId == circleId);
        }
        public async Task<PagedList<CircleMember>> GetCircleMemberList(PaginationParams paginationParams, int circleId, CircleRoleEnum? role = null)
        {
            var query = _context.CircleMembers.Include(cm => cm.AppUser).ThenInclude(ap => ap.MainPhoto)
                                            .Where(cm => cm.CircleId == circleId)
                                            // .Select(cm => cm.AppUser)
                                            .AsQueryable();
            if (role != null)
                query = query.Where(cm => cm.Role == role);

            return await PagedList<CircleMember>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<CircleEventParticipation>> GetCircleEventParticipationList(PaginationParams paginationParams, int eventId, CircleEventParticipationStatus? status = null)
        {
            var query = _context.CircleEventParticipations.Include(cm => cm.AppUser).ThenInclude(ap => ap.MainPhoto)
                                            .Where(cm => cm.CircleEventId == eventId)
                                            .OrderByDescending(cm => cm.DateCreated)
                                            // .Select(cm => cm.AppUser)
                                            .AsQueryable();

            if (status != null)
                query = query.Where(cep => cep.Status == status);

            return await PagedList<CircleEventParticipation>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<CircleMember>> GetLatestCircleMemberList(int circleId)
        {
            return await _context.CircleMembers.Include(cm => cm.AppUser).ThenInclude(ap => ap.MainPhoto)
                                            .Where(cm => cm.CircleId == circleId)
                                            .OrderByDescending(cm => cm.DateJoined)
                                            // .Select(cm => cm.AppUser)
                                            .ToListAsync();
        }
        public async Task<PagedList<Circle>> GetCirclesByUser(PaginationParams paginationParams, int userId)
        {
            var query = _context.Circles.Include(c => c.CircleMemberList)
                                         .Include(c => c.AppUser).ThenInclude(au => au.MainPhoto)
                                         .Include(c => c.City)
                                         .Include(c => c.MainPhoto)
                                         .Include(c => c.Category)
                                         .Where(c => c.CircleMemberList.Any(cm => cm.AppUserId == userId && cm.Role == CircleRoleEnum.MEMBER))
                                         .AsQueryable();
            return await PagedList<Circle>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<Circle>> GetCirclesOwnedByUser(PaginationParams paginationParams, int userId)
        {
            var query = _context.Circles.Include(c => c.CircleMemberList)
                                         .Include(c => c.AppUser).ThenInclude(au => au.MainPhoto)
                                         .Include(c => c.City)
                                         .Include(c => c.MainPhoto)
                                         .Include(c => c.Category)
                                         .Where(c => c.CircleMemberList.Any(cm => cm.AppUserId == userId && cm.Role == CircleRoleEnum.OWNER))
                                         .AsQueryable();
            return await PagedList<Circle>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<CircleEvent> GetCircleEvent(int id)
        {
            return await _context.CircleEvents.Include(ct => ct.AppUser).ThenInclude(au => au.MainPhoto)
                                             .Include(ct => ct.Circle)
                                             .Include(ct => ct.MainPhoto)
                                             .Include(ct => ct.Photos)
                                             .Include(ct => ct.City)
                                             .Include(ct => ct.CircleEventParticipations)
                                             .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task<CircleEvent> GetOldestCircleEvent(int circleId)
        {
            return await _context.CircleEvents.Include(ct => ct.AppUser).ThenInclude(au => au.MainPhoto)
                                             .Include(ct => ct.Circle)
                                             .Include(ct => ct.MainPhoto)
                                             .Include(ct => ct.Photos)
                                             .Include(ct => ct.City)
                                             .Include(ct => ct.CircleEventParticipations)
                                             .OrderBy(ct => ct.FromDate)
                                             .FirstOrDefaultAsync(ct => ct.CircleId == circleId);
        }

        public async Task<CircleTopic> GetCircleTopic(int id)
        {
            return await _context.CircleTopics.Include(ct => ct.AppUser).ThenInclude(au => au.MainPhoto)
                                             .Include(ct => ct.Circle)
                                             .Include(ct => ct.Photo)
                                             .FirstOrDefaultAsync(ct => ct.Id == id);
        }

        public async Task<PagedList<CircleTopic>> GetCircleTopicList(PaginationParams paginationParams, int circleId)
        {
            var query = _context.CircleTopics.Include(ct => ct.AppUser).ThenInclude(a => a.MainPhoto)
                                 .Include(ct => ct.TopicComments)
                                 .Include(ct => ct.Photo)
                                 .Where(ct => ct.CircleId == circleId)
                                 .OrderByDescending(ct => ct.DateCreated)
                                 .AsQueryable();

            return await PagedList<CircleTopic>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<CircleTopic>> GetLatestCircleTopicList(int circleId)
        {
            return await _context.CircleTopics.Include(ct => ct.AppUser).ThenInclude(a => a.MainPhoto)
                                 .Include(ct => ct.Photo)
                                 .Where(ct => ct.CircleId == circleId)
                                 .OrderByDescending(ct => ct.DateCreated)
                                 .ToListAsync();
        }

        public async Task<int> GetCircleEventCount(int circleId)
        {
            return await _context.CircleEvents.CountAsync(ce => ce.CircleId == circleId);
        }
        public async Task<IEnumerable<CircleEvent>> GetPastCircleEventList(int circleId)
        {
            return await _context.CircleEvents.Include(ct => ct.AppUser).ThenInclude(a => a.MainPhoto)
                                 .Include(ct => ct.MainPhoto)
                                 .Where(ct => ct.CircleId == circleId && ct.FromDate < DateTime.Now)
                                 .OrderByDescending(ct => ct.DateCreated)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<CircleEvent>> GetLatestCircleEventList(int circleId)
        {
            return await _context.CircleEvents.Include(ct => ct.AppUser).ThenInclude(a => a.MainPhoto)
                                 .Include(ct => ct.MainPhoto)
                                 .Where(ct => ct.CircleId == circleId)
                                 .Take(5)
                                 .OrderByDescending(ct => ct.DateCreated)
                                 .ToListAsync();
        }

        public async Task<PagedList<CircleTopic>> GetCircleTopicByUser(PaginationParams paginationParams, int circleId, int userId)
        {
            var query = _context.CircleTopics.Include(ct => ct.AppUser).ThenInclude(a => a.MainPhoto)
                                             .Include(ct => ct.TopicComments)
                                             .Include(ct => ct.Photo)
                                             .Where(ct => ct.CircleId == circleId && ct.AppUserId == userId)
                                             .OrderByDescending(ct => ct.DateCreated)
                                             .AsQueryable();

            return await PagedList<CircleTopic>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<int> GetCircleTopicCommentCount(int circleTopicId)
        {
            return await _context.CircleTopicComments.Where(tc => tc.CircleTopicId == circleTopicId).CountAsync();
        }

        public async Task<CircleTopicComment> GetCircleTopicComment(int id)
        {
            return await _context.CircleTopicComments.Include(tc => tc.AppUser).ThenInclude(au => au.MainPhoto)
                                                     .Include(tc => tc.Photo).FirstOrDefaultAsync(ctc => ctc.Id == id);
        }

        public async Task<PagedList<CircleTopicComment>> GetCircleTopicCommentList(PaginationParams paginationParams, int circleTopicId)
        {
            var query = _context.CircleTopicComments.Include(tc => tc.AppUser).ThenInclude(a => a.MainPhoto)
                                                    // .Include(tc => tc.Replies)
                                                    .Include(tc => tc.Photo)
                                                    .Where(tc => tc.CircleTopicId == circleTopicId)
                                                    .OrderByDescending(tc => tc.DateCreated)
                                                    .AsQueryable();

            return await PagedList<CircleTopicComment>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<List<Photo>> GetAllCommentPhotosForCircleTopic(int circleTopicId)
        {
            var circleTopicComments = await _context.CircleTopicComments.Include(ctc => ctc.Photo)
                                                                    .Where(ctc => ctc.CircleTopicId == circleTopicId && ctc.PhotoId != null)
                                                                    .ToListAsync();
            List<Photo> photos = new List<Photo>();
            foreach (var topicComment in circleTopicComments)
            {
                photos.Add(topicComment.Photo);
            }
            return photos;
        }

        public async Task<CircleTopicCommentReply> GetCircleTopicCommentReply(int circleTopicCommentReplyId)
        {
            return await _context.CircleTopicCommentReplies.FirstOrDefaultAsync(tr => tr.Id == circleTopicCommentReplyId);
        }

        public async Task<IEnumerable<CircleTopicCommentReply>> GetCircleTopicCommentReplies(int circleTopicCommentId)
        {
            return await _context.CircleTopicCommentReplies.Include(cr => cr.AppUser).ThenInclude(a => a.MainPhoto)
                                                    .Where(tr => tr.CommentId == circleTopicCommentId).OrderBy(tr => tr.DateCreated).ToListAsync();
        }

        public async Task<int> GetCircleTopicCommentReplyCount(int circleTopicCommentId)
        {
            return await _context.CircleTopicCommentReplies.Where(tc => tc.CommentId == circleTopicCommentId).CountAsync();
        }

        public async Task<IEnumerable<Circle>> GetLatestCircles()
        {
            return await _context.Circles.Include(c => c.AppUser).ThenInclude(au => au.MainPhoto)
                                    .Include(c => c.City)
                                    .Include(c => c.MainPhoto).OrderByDescending(c => c.DateCreated).Take(6).ToListAsync();
        }

        public async Task<int> GetCircleEventParticipationCount(int eventId)
        {
            return await _context.CircleEventParticipations.CountAsync(cep => cep.CircleEventId == eventId);
        }

        public async Task<bool> IsEventFull(int eventId)
        {
            var temp = await _context.CircleEvents.FirstOrDefaultAsync(ce => ce.Id == eventId);
            return temp != null && temp.MaxNumber != null ? await GetCircleEventParticipationCount(temp.Id) >= temp.MaxNumber : false;
        }

        public async Task<bool> IsMember(int appUserId, int circleId)
        {
            return await _context.CircleMembers.AnyAsync(cm => cm.AppUserId == appUserId && cm.CircleId == circleId);
        }

        public async Task<bool> IsOwner(int appUserId, int circleId)
        {
            return await _context.CircleMembers.AnyAsync(cm => cm.AppUserId == appUserId && cm.CircleId == circleId && cm.Role == CircleRoleEnum.OWNER);
        }

        public async Task<CircleRequest> GetCircleRequest(int appUserId, int circleId)
        {
            return await _context.CircleRequests.Include(cr => cr.Circle).Include(cr => cr.AppUser).Where(cr => cr.AppUserId == appUserId && cr.CircleId == circleId).FirstOrDefaultAsync();
        }

        public async Task<bool> RequestSent(int appUserId, int circleId)
        {
            return await _context.CircleRequests.AnyAsync(cr => cr.AppUserId == appUserId && cr.CircleId == circleId);
        }

        public async Task ApproveAll(int circleId)
        {
            var requests = await _context.CircleRequests.Where(cr => cr.CircleId == circleId && !cr.Declined).ToListAsync();
            foreach (var request in requests)
            {
                Add(new CircleMember()
                {
                    AppUserId = request.AppUserId,
                    CircleId = request.CircleId,
                    Role = CircleRoleEnum.MEMBER
                });
                Delete(request);
            }
        }
        public async Task<IEnumerable<CircleRequest>> GetRequestsForCircle(int circleId)
        {
            return await _context.CircleRequests.Include(cr => cr.AppUser).ThenInclude(a => a.MainPhoto).Where(cr => cr.CircleId == circleId).ToListAsync();
        }
    }
}