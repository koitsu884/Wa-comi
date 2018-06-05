using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Dto;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly ApplicationDbContext _context;
        public DataRepository(ApplicationDbContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public void DeleteAll<T>(T entities) where T : class
        {
            _context.RemoveRange(entities);
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<IEnumerable<HomeTown>> GetHomeTowns()
        {
            return await _context.HomeTowns.ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id)
        {
            className = className.ToLower();
            switch (className)
            {
                case "member":
                    var member = await _context.Members.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
                    if (member != null)
                    {
                        return member.Photos.ToList();
                    }
                    break;
                case "business":
                    var business = await _context.BusinessUsers.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
                    if (business != null)
                    {
                        return business.Photos.ToList();
                    }
                    break;
                case "clanseek":
                    var clanSeek = await _context.ClanSeeks.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
                    if (clanSeek != null)
                    {
                        return clanSeek.Photos.ToList();
                    }
                    break;
            }
            return null;
        }
        public async Task<UserBase> GetUser(string type, int id)
        {
            switch (type.ToLower())
            {
                case "member":
                    return await GetMember(id);
                case "business":
                    return await GetBusinessUser(id);
                default:
                    return null;
            }
        }

        public async Task<BusinessUser> GetBusinessUser(int id)
        {
            return await _context.BusinessUsers.Include(m => m.City)
                                        .Include(m => m.Identity)
                                        .Include(m => m.Photos)
                                        .Include(m => m.Blogs)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        }

        
        public async Task<bool> MemberExist(int memberId)
        {
            return await _context.Members.AnyAsync(m => m.Id == memberId);
        }
        public async Task<Member> GetMember(int id)
        {
            return await _context.Members.Include(m => m.City)
                                        .Include(m => m.HomeTown)
                                        .Include(m => m.Identity)
                                        .Include(m => m.Photos)
                                        .Include(m => m.Blogs)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<Member> GetMemberByIdentityId(string id)
        {
            return await _context.Members.Include(m => m.City)
                                        .Include(m => m.HomeTown)
                                        .Include(m => m.Identity)
                                        .Include(m => m.Photos)
                                        .Include(m => m.Blogs)
                                        .FirstOrDefaultAsync(m => m.IdentityId == id);
        }
        public async Task<IEnumerable<Member>> GetMembers(UserParams userParams)
        {
            return await _context.Members.Where(m => m.IsActive == true).ToListAsync();
        }

        public async Task<Blog> GetBlog(int id)
        {
            return await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Blog>> GetBlogsForClass(string className, int id)
        {
            className = className.ToLower();
            switch (className)
            {
                case "member":
                    var member = await _context.Members.Include(m => m.Blogs).FirstOrDefaultAsync(m => m.Id == id);
                    if (member != null)
                    {
                        return member.Blogs.ToList();
                    }
                    break;
                case "business":
                    var business = await _context.BusinessUsers.Include(b => b.Blogs).FirstOrDefaultAsync(b => b.Id == id);
                    if (business != null)
                    {
                        return business.Blogs.ToList();
                    }
                    break;
            }
            return null;
        }

        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await _context.Blogs.Where(b => b.IsActive == true).ToListAsync();
        }

        public async Task<BlogFeed> GetLatestBlogFeed(Blog blog)
        {
            return await _context.BlogFeeds.Where(bf => bf.BlogId == blog.Id).OrderByDescending(bf => bf.Id).FirstOrDefaultAsync();
        }

        public async Task<BlogFeed> GetBlogFeed(int id)
        {
            return await _context.BlogFeeds.FirstOrDefaultAsync(bf => bf.Id == id);
        }

        public async Task<IEnumerable<BlogFeed>> GetLatestBlogFeeds()
        {
            return await _context.BlogFeeds.Include(bf => bf.Blog)
                                            .Where(bf => bf.Blog.IsActive == true)
                                            .OrderByDescending(bf => bf.PublishingDate)
                                            .Take(50).ToListAsync();
        }

        public async Task<ClanSeek> GetClanSeek(int id)
        {
            return await _context.ClanSeeks.Include(cs => cs.Category)
                                           .Include(cs => cs.Member)
                                           .Include(cs => cs.Member.Identity)
                                           .Include(cs => cs.Location)
                                           .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<ClanSeekCategory>> GetClanSeekCategories()
        {
            return await _context.ClanSeekCategories.ToListAsync();
        }

        public async Task<IEnumerable<ClanSeek>> GetClanSeeks(int? categoryId = null, int? cityId = null, bool? latest = null)
        {
            var clanSeeks = _context.ClanSeeks.Include(cs => cs.Category)
                                           .Include(cs => cs.Member)
                                           .Include(cs => cs.Member.Identity)
                                           .Include(cs => cs.Location)
                                           .OrderByDescending(cs => cs.LastActive)
                                           .AsQueryable();

            if (latest != null)
            {
                clanSeeks = clanSeeks.Take(6);
            }

            if (categoryId != null)
            {
                clanSeeks = clanSeeks.Where(cs => cs.CategoryId == categoryId);
            }

            if (cityId != null)
            {
                clanSeeks = clanSeeks.Where(cs => cs.LocationId == cityId);
            }
            return await clanSeeks.ToListAsync();
        }

        public async Task<PropertySeek> GetPropertySeek(int id)
        {
            return await _context.PropertySeeks.FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<PropertySeek>> GetPropertySeeks(int? categoryId = null)
        {
            if (categoryId == null)
                return await _context.PropertySeeks.ToListAsync();
            return await _context.PropertySeeks.Where(ps => ps.CategoryId == categoryId).ToListAsync();
        }

        public async Task<IEnumerable<PropertySeekCategory>> GetPropertySeekCategories()
        {
            return await _context.PropertySeekCategories.ToListAsync();
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
            return topic.Title;
        }

        public async Task<DailyTopic> GetOldestDailyTopic()
        {
            return await _context.DailyTopics.OrderBy(dt => dt.LastDiscussed).FirstOrDefaultAsync();
        }

        public async Task<int> GetPostedTopicCountForUser(string userId)
        {
            var records = await _context.DailyTopics.Where(dt => dt.UserId == userId).ToListAsync();
            return records.Count();
        }

        public async Task<TopicLike> GetTopicLike(string userId, int recordId)
        {
            return await _context.TopicLikes.FirstOrDefaultAsync(tl => tl.SupportUserId == userId && tl.DailyTopicId == recordId);
        }

        public async Task<IEnumerable<DailyTopic>> GetDailyTopicList()
        {
            return await _context.DailyTopics.Include(dt => dt.TopicLikes).Where(dt => dt.IsActive == false).OrderByDescending(dt => dt.TopicLikes.Count()).ToListAsync();
        }

        public async Task<IEnumerable<TopicLike>> GetTopicLikesForUser(string userId)
        {
            return await _context.TopicLikes.Where(tl => tl.SupportUserId == userId).ToListAsync();
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
            return await _context.TopicComments.Include(tc => tc.TopicCommentFeels)
                                            .Include(tc => tc.TopicReplies)
                                               //    .Include(tc => tc.Member)
                                               //    .Include(tc => tc.Member.Identity)
                                               .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<IEnumerable<TopicComment>> GetTopicCommentsForMember(int memberId)
        {
            return await _context.TopicComments.Include(tc => tc.TopicCommentFeels)
                                                .Include(tc => tc.TopicReplies)
                                               .Include(tc => tc.Member)
                                               //    .Include(tc => tc.Member.Identity)
                                               .Where(tc => tc.MemberId == memberId)
                                               .ToListAsync();
        }

        public async Task<IEnumerable<TopicComment>> GetLatestTopicCommentList()
        {
            return await _context.TopicComments.Include(tc => tc.TopicCommentFeels)
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
                                               //    .Include(tc => tc.Member)
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
            return await _context.TopicReplies.FirstOrDefaultAsync(tr => tr.Id == id);
        }

        public async Task<IEnumerable<TopicReply>> GetTopicRepliesByCommentId(int commentId)
        {
            return await _context.TopicReplies.Where(tr => tr.TopicCommentId == commentId).ToListAsync();
        }

        public async Task<TopicCommentFeel> GetCommentFeel(int memberId, int commentId)
        {
            return await _context.TopicCommentFeels.Where(tcf => tcf.MemberId == memberId && tcf.CommentId == commentId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TopicCommentFeel>> GetCommentFeels(int memberId = 0)
        {
            if(memberId > 0)
               return await _context.TopicCommentFeels.Where(tcf => tcf.MemberId == memberId).ToListAsync();
            return await _context.TopicCommentFeels.ToListAsync();
        }

        public async Task<Friend> GetFriend(int memberId, int friendId)
        {
            return await _context.Friends.Include(f => f.Member)
                                         .Include(f => f.Member.Identity)
                                         .FirstOrDefaultAsync(f => f.MemberId == memberId && f.FriendMemberid == friendId);
        }

        public async Task<IEnumerable<Friend>> GetFriends(int memberId)
        {
            return await _context.Friends.Include(f => f.Member)
                                         .Include(f => f.Member.Identity)
                                         .Where(f => f.MemberId == memberId)
                                         .ToListAsync();
        }

        public async Task<FriendRequest> GetFriendRequest(int senderId, int recipientId)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.SenderId == senderId && fr.RecipientId == recipientId);
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsReceived(int memberId)
        {
            return await _context.FriendRequests.Include(fr => fr.Sender)
                                                .Include(fr => fr.Sender.Identity)
                                                .Where(fr => fr.RecipientId == memberId)
                                                .ToListAsync();
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int memberId)
        {
            return await _context.FriendRequests.Include(fr => fr.Recipient)
                                                .Include(fr => fr.Recipient.Identity)
                                                .Where(fr => fr.SenderId == memberId)
                                                .ToListAsync();
        }

        public async Task<FriendRequest> GetFriendRequestFrom(int memberId, int senderId)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.RecipientId == memberId && fr.SenderId == senderId);
        }
    }
}