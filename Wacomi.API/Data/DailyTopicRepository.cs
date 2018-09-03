using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class DailyTopicRepository : RepositoryBase, IDailyTopicRepository
    {
        public DailyTopicRepository(ApplicationDbContext context) : base(context)
        {
        }

         public async Task<DailyTopic> GetDailyTopic(int id)
        {
            return await _context.DailyTopics.FirstOrDefaultAsync(dt => dt.Id == id);
        }


        public async Task<DailyTopic> GetActiveDailyTopic()
        {
            return await _context.DailyTopics.Where(dt => dt.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task<DailyTopic> GetTopDailyTopic()
        {
            var topTopicId = await (
                                from g in _context.TopicLikes
                                group g by g.DailyTopicId into g
                                join dt in _context.DailyTopics
                                on g.Key equals dt.Id
                                orderby g.Count() descending, dt.LastDiscussed descending
                                select g.Key
                              ).FirstOrDefaultAsync();
            ;
            return await _context.DailyTopics.FirstOrDefaultAsync(dt => dt.Id == topTopicId);
        }

        public async Task<string> GetTodaysTopic()
        {
            var topic = await _context.DailyTopics.FirstOrDefaultAsync(dt => dt.IsActive == true);
            if (topic == null)
            {
                topic = await _context.DailyTopics.FirstOrDefaultAsync();
                topic.IsActive = true;
                var result = await _context.SaveChangesAsync();
            }
            return topic.Title;
        }

        public async Task<DailyTopic> GetOldestDailyTopic()
        {
            return await _context.DailyTopics.OrderBy(dt => dt.LastDiscussed).FirstOrDefaultAsync();
        }

        public async Task<int> GetPostedTopicCountForUser(int userId)
        {
            var records = await _context.DailyTopics.Where(dt => dt.UserId == userId).ToListAsync();
            return records.Count();
        }


        public async Task<TopicLike> GetTopicLike(int userId, int recordId)
        {
            return await _context.TopicLikes.FirstOrDefaultAsync(tl => tl.SupportAppUserId == userId && tl.DailyTopicId == recordId);
        }

        public async Task<IEnumerable<DailyTopic>> GetDailyTopicList()
        {
            return await _context.DailyTopics.Include(dt => dt.TopicLikes).Where(dt => dt.IsActive == false).OrderByDescending(dt => dt.TopicLikes.Count()).ToListAsync();
        }

        public async Task<IEnumerable<TopicLike>> GetTopicLikesForUser(int userId)
        {
            return await _context.TopicLikes.Where(tl => tl.SupportAppUserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<TopicLike>> GetTopicLikesForTopic(int topicId)
        {
            return await _context.TopicLikes.Where(tl => tl.DailyTopicId == topicId).ToListAsync();
        }

        public void ResetTopicLikes(int topicId)
        {
            var topicLikes = _context.TopicLikes.Where(tl => tl.DailyTopicId == topicId);
            _context.RemoveRange(topicLikes);
        }

        //=============================================================
        // Daily Topic Comment
        //=============================================================
        public async Task<TopicComment> GetTopicComment(int id)
        {
            return await _context.TopicComments.Include(tc => tc.AppUser)
                                                .ThenInclude(u => u.MainPhoto)
                                             .Include(tc => tc.TopicCommentFeels)
                                            .Include(tc => tc.TopicReplies)
                                               //    .Include(tc => tc.Member)
                                               //    .Include(tc => tc.Member.Identity)
                                               .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<IEnumerable<TopicComment>> GetTopicCommentsForMember(int userId)
        {
            return await _context.TopicComments.Include(tc => tc.TopicCommentFeels)
                                                .Include(tc => tc.TopicReplies)
                                               .Include(tc => tc.AppUser)
                                                .ThenInclude(u => u.MainPhoto)
                                               //    .Include(tc => tc.Member.Identity)
                                               .Where(tc => tc.AppUserId == userId)
                                               .ToListAsync();
        }

        public async Task<IEnumerable<TopicComment>> GetLatestTopicCommentList()
        {
            return await _context.TopicComments.Include(tc => tc.AppUser)
                                                    .ThenInclude(u => u.MainPhoto)
                                                .Include(tc => tc.TopicCommentFeels)
                                                .Include(tc => tc.TopicReplies)
                                               //    .Include(tc => tc.Member)
                                               //    .Include(tc => tc.Member.Identity)
                                               .OrderByDescending(tc => tc.Id)
                                               .Take(10)
                                               .ToListAsync();
        }

        public async Task<IEnumerable<TopicComment>> GetTopicComments()
        {
            return await _context.TopicComments.Include(tc => tc.TopicCommentFeels)
                                                .Include(tc => tc.TopicReplies)
                                                .Include(tc => tc.AppUser)
                                                    .ThenInclude(u => u.MainPhoto)
                                               //    .Include(tc => tc.Member.Identity)
                                               .OrderByDescending(tc => tc.Id)
                                               .Take(1000)
                                               .ToListAsync();
        }

        public void ResetTopicComments()
        {
            _context.TopicComments.RemoveRange(_context.TopicComments);
        }

        //====================================================================
        // Daily Topic Reply
        //====================================================================
        public async Task<bool> TopicCommentExists(int id)
        {
            return await _context.TopicComments.AnyAsync(tc => tc.Id == id);
        }
        public async Task<TopicReply> GetTopicReply(int id)
        {
            return await _context.TopicReplies
                            .Include(tr => tr.AppUser)
                            .ThenInclude(u => u.MainPhoto)
                            .FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task<IEnumerable<TopicReply>> GetTopicRepliesByCommentId(int commentId)
        {
            return await _context.TopicReplies
                        .Include(tr => tr.AppUser)
                        .ThenInclude(u => u.MainPhoto)
                        .Where(tr => tr.TopicCommentId == commentId).ToListAsync();
        }

        public async Task<TopicCommentFeel> GetCommentFeel(int userId, int commentId)
        {
            return await _context.TopicCommentFeels
                        .Include(tcf => tcf.AppUser)
                        .ThenInclude(u => u.MainPhoto)
                        .Where(tcf => tcf.AppUserId == userId && tcf.CommentId == commentId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TopicCommentFeel>> GetCommentFeels(int userId = 0)
        {
            if (userId > 0)
                return await _context.TopicCommentFeels
                            .Include(tcf => tcf.AppUser)
                            .ThenInclude(u => u.MainPhoto)
                            .Where(tcf => tcf.AppUserId == userId).ToListAsync();
            return await _context.TopicCommentFeels.ToListAsync();
        }
    }
}