using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async Task<bool> RecordExist(string recordType, int id)
        {
            switch (recordType.ToLower())
            {
                case "appuser":
                    return await _context.AppUsers.AnyAsync(r => r.Id == id);
                case "businessprofile":
                    return await _context.BusinessProfiles.AnyAsync(r => r.Id == id);
                case "memberprofile":
                    return await _context.MemberProfiles.AnyAsync(r => r.Id == id);
                case "clanseek":
                    return await _context.ClanSeeks.AnyAsync(r => r.Id == id);
                case "dailytopic":
                    return await _context.DailyTopics.AnyAsync(r => r.Id == id);
                case "blog":
                    return await _context.Blogs.AnyAsync(r => r.Id == id);
                case "blogfeed":
                    return await _context.BlogFeeds.AnyAsync(r => r.Id == id);
                case "topiccomment":
                    return await _context.TopicComments.AnyAsync(r => r.Id == id);
                case "photo":
                    return await _context.Photos.AnyAsync(r => r.Id == id);

            }
            return false;
        }

        public async Task<IEnumerable<City>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        public async Task<IEnumerable<HomeTown>> GetHomeTowns()
        {
            return await _context.HomeTowns.ToListAsync();
        }

        public async Task<int> SaveAll()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Photo> GetPhoto(int id)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Photo> GetPhotoByPublicId(string publicId)
        {
            return await _context.Photos.FirstOrDefaultAsync(p => p.PublicId == publicId);
        }

        public async Task<IEnumerable<Photo>> GetPhotosForRecord(string recordType, int recordId)
        {
            switch (recordType.ToLower())
            {
                case "appuser":
                    var appUser = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (appUser != null)
                    {
                        return appUser.Photos.ToList();
                    }
                    break;
                case "clanseek":
                    var clanSeek = await _context.ClanSeeks.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == recordId);
                    if (clanSeek != null)
                    {
                        return clanSeek.Photos.ToList();
                    }
                    break;
            }
            return null;
        }
        public async Task<IEnumerable<Photo>> GetPhotosForAppUser(int id)
        {
            var appUser = await _context.AppUsers.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
            if (appUser != null)
            {
                return appUser.Photos.ToList();
            }
            return null;
        }

        // public async Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id)
        // {
        //     className = className.ToLower();
        //     switch (className)
        //     {
        //         case "member":
        //             var member = await _context.Members.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
        //             if (member != null)
        //             {
        //                 return member.Photos.ToList();
        //             }
        //             break;
        //         case "business":
        //             var business = await _context.BusinessUsers.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
        //             if (business != null)
        //             {
        //                 return business.Photos.ToList();
        //             }
        //             break;
        //         case "clanseek":
        //             var clanSeek = await _context.ClanSeeks.Include(p => p.Photos).FirstOrDefaultAsync(m => m.Id == id);
        //             if (clanSeek != null)
        //             {
        //                 return clanSeek.Photos.ToList();
        //             }
        //             break;
        //     }
        //     return null;
        // }

        public async Task<AppUser> GetAppUser(int id)
        {
            return await _context.AppUsers.Include(au => au.City)
                                          .Include(au => au.Photos)
                                          .FirstOrDefaultAsync(au => au.Id == id);
        }


        public async Task<AppUser> GetAppUserByAccountId(string accountId)
        {
            return await _context.AppUsers.Include(au => au.City)
                                          .Include(au => au.Photos)
                                         //   .Include(au => au.Blogs)
                                         .FirstOrDefaultAsync(au => au.AccountId == accountId);
        }

        public async Task AddLikeCountToUser(int userId)
        {
            var appUser = await GetAppUser(userId);
            if (appUser != null)
                appUser.TotalLike++;
            //TODO: add for month, week
        }


        public async Task<BusinessProfile> GetBusinessProfile(int id)
        {
            return await _context.BusinessProfiles.Include(b => b.AppUser)
                                        .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<MemberProfile> GetMemberProfile(int id)
        {
            return await _context.MemberProfiles.Include(m => m.HomeTown)
                                                .Include(m => m.AppUser)
                                                // .Include(m => m.AppUser.Photos)
                                                // .Include(m => m.AppUser.Blogs)
                                                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MemberProfile> GetMemberProfileByAccountId(string id)
        {
            return await _context.MemberProfiles.Include(m => m.HomeTown)
                                                .Include(m => m.AppUser)
                                                .FirstOrDefaultAsync(m => m.AppUser.AccountId == id);
        }
        // public async Task<IEnumerable<MemberProfile>> GetMemberProfiles(UserParams userParams)
        // {
        //     return await _context.MemberProfiles.Where(m => m.IsActive == true).ToListAsync();
        // }

        public async Task<Blog> GetBlog(int id)
        {
            return await _context.Blogs.Include(b => b.Photo).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<int> GetBlogCountForUser(int id)
        {
            return await _context.Blogs.Where(b => b.OwnerId == id).CountAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsForUser(int id)
        {
            return await _context.Blogs.Include(b => b.Photo).Where(b => b.OwnerId == id).ToListAsync();
        }

        // public async Task<IEnumerable<Blog>> GetBlogsForClass(string className, int id)
        // {
        //     className = className.ToLower();
        //     switch (className)
        //     {
        //         case "member":
        //             var member = await _context.Members.Include(m => m.Blogs).FirstOrDefaultAsync(m => m.Id == id);
        //             if (member != null)
        //             {
        //                 return member.Blogs.ToList();
        //             }
        //             break;
        //         case "business":
        //             var business = await _context.BusinessUsers.Include(b => b.Blogs).FirstOrDefaultAsync(b => b.Id == id);
        //             if (business != null)
        //             {
        //                 return business.Blogs.ToList();
        //             }
        //             break;
        //     }
        //     return null;
        // }

        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await _context.Blogs.Include(b => b.Photo).Where(b => b.IsActive == true).ToListAsync();
        }

        public async Task<IEnumerable<Blog>> GetBlogsForRssFeed(int count = 100)
        {
            return await _context.Blogs.Where(b => b.IsActive == true && !string.IsNullOrEmpty(b.RSS))
                                       .OrderBy(b => b.DateRssRead)
                                       .Take(count)
                                       .ToListAsync();
        }

        public async Task<BlogFeed> GetLatestBlogFeed(int blogId)
        {
            return await _context.BlogFeeds
                            .Include(bf => bf.Photo)
                            .Include(bf => bf.Blog)
                            .ThenInclude(b => b.Photo)
                            .Where(bf => bf.BlogId == blogId).OrderByDescending(bf => bf.PublishingDate).FirstOrDefaultAsync();
        }

        public async Task<BlogFeed> GetBlogFeed(int id)
        {
            return await _context.BlogFeeds.Include(bf => bf.Photo)
                                           .Include(bf => bf.Blog)
                                           .ThenInclude(b => b.Photo)
                                           .Include(bf => bf.FeedLikes)
                                           .Include(bf => bf.FeedComments)
                                           .FirstOrDefaultAsync(bf => bf.Id == id);
        }

        public async Task<bool> BlogFeedLiked(int appUserId, int blogFeedId)
        {
            return await _context.BlogFeedLikes.AnyAsync(bfl => bfl.SupportAppUserId == appUserId && bfl.BlogFeedId == blogFeedId);
        }

        public async Task<BlogFeedLike> GetBlogFeedLike(int id)
        {
            return await _context.BlogFeedLikes.FirstOrDefaultAsync(bfl => bfl.Id == id);
        }

        public async Task<IEnumerable<BlogFeedLike>> GetBlogFeedLikesForUser(int userId)
        {
            return await _context.BlogFeedLikes.Where(bfl => bfl.SupportAppUserId == userId).ToListAsync();
        }

        public async Task<BlogFeedComment> GetBlogFeedComment(int id)
        {
            return await _context.BlogFeedComments.FirstOrDefaultAsync(bfc => bfc.Id == id);
        }

        public async Task<IEnumerable<BlogFeedComment>> GetBlogFeedCommentsForFeed(int feedId)
        {
            return await _context.BlogFeedComments.Include(bf => bf.AppUser).ThenInclude(u => u.MainPhoto)
                                                  .Where(bfc => bfc.BlogFeedId == feedId).ToListAsync();
        }
        public async Task<IEnumerable<BlogFeed>> GetLatestBlogFeeds()
        {
            return await _context.BlogFeeds.Include(bf => bf.Photo)
                                            .Include(bf => bf.Blog)
                                            .ThenInclude(b => b.Photo)
                                            .Where(bf => bf.Blog.IsActive == true && bf.IsActive == true)
                                            .OrderByDescending(bf => bf.PublishingDate)
                                            .Take(12)
                                            .ToListAsync();
        }

        public async Task<PagedList<BlogFeed>> GetBlogFeeds(PaginationParams paginationParams, string category = null, int? userId = null)
        {
            var blogFeeds = _context.BlogFeeds.Include(bf => bf.Photo)
                                            .Include(bf => bf.Blog).ThenInclude(b => b.Owner)
                                            .Include(bf => bf.Blog).ThenInclude(b => b.Photo)
                                            .Include(bf => bf.FeedLikes)
                                            .Include(bf => bf.FeedComments)
                                            .OrderByDescending(bf => bf.PublishingDate)
                                           .AsQueryable();

            blogFeeds = blogFeeds.Where(bf => bf.Blog.IsActive == true && bf.IsActive == true);

            if (!string.IsNullOrEmpty(category))
            {
                blogFeeds = blogFeeds.Where(bf => bf.Blog.Category == category
                                               || bf.Blog.Category2 == category
                                               || bf.Blog.Category3 == category);
            }

            if (userId != null)
            {
                blogFeeds = blogFeeds.Where(bf => bf.Blog.OwnerId == userId);
            }

            return await PagedList<BlogFeed>.CreateAsync(blogFeeds, paginationParams.PageNumber, paginationParams.PageSize);
            //return await blogFeeds.ToListAsync();
        }
        public async Task<IEnumerable<BlogFeed>> GetBlogFeedsByBlogId(int blogId)
        {
            return await _context.BlogFeeds.Include(bf => bf.FeedLikes)
                                            .Include(bf => bf.FeedComments)
                                            .Include(bf => bf.Photo)
                                            .Where(bf => bf.BlogId == blogId && bf.IsActive == true)
                                            .OrderByDescending(bf => bf.PublishingDate)
                                            .Take(20).ToListAsync();
        }

        public async Task<int> GetBlogFeedsCountForBlog(int blogId)
        {
            return await _context.BlogFeeds.Where(bf => bf.BlogId == blogId).CountAsync();
        }

        public async Task<IEnumerable<BlogFeed>> GetBlogFeeds(DateTime? from = null, DateTime? to = null)
        {
            var query = _context.BlogFeeds.Include(bf => bf.Photo).OrderByDescending(bf => bf.PublishingDate).AsQueryable();
            if (from != null && to != null && from > to)
            {
                from = null;
            }

            if (from != null)
                query = query.Where(bf => bf.PublishingDate >= from);
            if (to != null)
                query = query.Where(bf => bf.PublishingDate <= to);

            return await query.ToListAsync();
        }

        // public async Task DeleteOldFeeds(){
        //     var targetDate = DateTime.Now.AddMonths(-6);
        //     var deletingFeeds = await _context.BlogFeeds.Where(bf => bf.PublishingDate <= targetDate).ToListAsync();
        //   //  _context.BlogFeeds.RemoveRange(deletingFeeds);
        //     foreach(var feed in deletingFeeds){
        //         await DeleteFeedLikes(feed.Id);
        //         await DeleteFeedComments(feed.Id);
        //         _context.BlogFeeds.Remove(feed);
        //     }
        // }

        public async Task DeleteFeed(BlogFeed feed)
        {
            await DeleteFeedLikes(feed.Id);
            await DeleteFeedComments(feed.Id);
            Delete(feed.Photo);
            Delete(feed);
        }
        public async Task DeleteFeeds(DateTime? targetDate = null)
        {
            var query = _context.BlogFeeds.AsQueryable();
            if (targetDate != null)
                query = query.Where(bf => bf.PublishingDate <= targetDate);
            var deletingFeeds = await query.ToListAsync();
            foreach (var feed in deletingFeeds)
            {
                await DeleteFeeWithRelatingTablesAndFiles(feed);
            }
        }

        private async Task DeleteFeeWithRelatingTablesAndFiles(BlogFeed feed)
        {
            await DeleteFeedLikes(feed.Id);
            await DeleteFeedComments(feed.Id);
            // var fileName = System.IO.Path.GetFileName(feed.ImageUrl);
            // var physicalImagePath = System.IO.Path.Combine(feedImageFolder, feed.BlogId.ToString(), fileName);
            // System.IO.File.Delete(physicalImagePath);
            _context.Photos.Remove(feed.Photo);
            _context.BlogFeeds.Remove(feed);
        }



        public async Task DeleteFeedLikes(int feedId)
        {
            var deletingFeedLikes = await _context.BlogFeedLikes.Where(bfl => bfl.BlogFeedId == feedId).ToListAsync();
            _context.BlogFeedLikes.RemoveRange(deletingFeedLikes);
        }

        public async Task DeleteFeedComments(int feedId)
        {
            var deletingFeedComments = await _context.BlogFeedComments.Where(bfl => bfl.BlogFeedId == feedId).ToListAsync();
            _context.BlogFeedComments.RemoveRange(deletingFeedComments);
        }

        public async Task DeleteFeedsForBlog(int blogId)
        {
            var deletingFeeds = await _context.BlogFeeds.Where(bf => bf.BlogId == blogId).ToListAsync();
            foreach (var feed in deletingFeeds)
            {
                await DeleteFeeWithRelatingTablesAndFiles(feed);
            }
        }
        public async Task<ClanSeek> GetClanSeek(int id)
        {
            return await _context.ClanSeeks.Include(cs => cs.Category)
                                           .Include(cs => cs.AppUser)
                                           .Include(cs => cs.Location)
                                           .Include(cs => cs.MainPhoto)
                                           .Include(cs => cs.Photos)
                                           .FirstOrDefaultAsync(cs => cs.Id == id);
        }

        public async Task<IEnumerable<ClanSeekCategory>> GetClanSeekCategories()
        {
            return await _context.ClanSeekCategories.ToListAsync();
        }

        //public async Task<PagedList<ClanSeek>> GetClanSeeks(int? categoryId = null, int? cityId = null, bool? latest = null)
        public async Task<PagedList<ClanSeek>> GetClanSeeks(PaginationParams paginationParams, int? categoryId = null, int? cityId = null)
        {
            var clanSeeks = _context.ClanSeeks.Include(cs => cs.Category)
                                          .Include(cs => cs.AppUser)
                                            .ThenInclude(u => u.MainPhoto)
                                          .Include(cs => cs.MainPhoto)
                                          .Include(cs => cs.Location)
                                          .OrderByDescending(cs => cs.LastActive)
                                          .AsQueryable();
            if (categoryId != null)
            {
                clanSeeks = clanSeeks.Where(cs => cs.CategoryId == categoryId);
            }

            if (cityId != null)
            {
                clanSeeks = clanSeeks.Where(cs => cs.LocationId == cityId);
            }
            clanSeeks = clanSeeks.Where(cs => cs.IsActive == true);

            return await PagedList<ClanSeek>.CreateAsync(clanSeeks, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<ClanSeek>> GetClanSeeksByUser(int userId)
        {
            return await _context.ClanSeeks.Include(cs => cs.Category)
                                           .Include(cs => cs.Location)
                                           .Include(cs => cs.MainPhoto)
                                           .Where(cs => cs.AppUserId == userId)
                                           .OrderByDescending(cs => cs.LastActive).ToListAsync();
        }

        public async Task<int> GetClanSeeksCountByUser(int userId)
        {
            return await _context.ClanSeeks.Where(cs => cs.AppUserId == userId).CountAsync();
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

        public async Task<Friend> GetFriend(int memberId, int friendId)
        {
            return await _context.Friends.Include(f => f.Member)
                                         .Include(f => f.Member.AppUser)
                                         .FirstOrDefaultAsync(f => f.MemberId == memberId && f.FriendMemberid == friendId);
        }

        public async Task<IEnumerable<Friend>> GetFriends(int memberId)
        {
            return await _context.Friends.Include(f => f.Member)
                                         .Include(f => f.Member.AppUser)
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
                                                .Include(fr => fr.Sender.AppUser)
                                                .Where(fr => fr.RecipientId == memberId)
                                                .ToListAsync();
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int memberId)
        {
            return await _context.FriendRequests.Include(fr => fr.Recipient)
                                                .Include(fr => fr.Recipient.AppUser)
                                                .Where(fr => fr.SenderId == memberId)
                                                .ToListAsync();
        }

        public async Task<FriendRequest> GetFriendRequestFrom(int memberId, int senderId)
        {
            return await _context.FriendRequests.FirstOrDefaultAsync(fr => fr.RecipientId == memberId && fr.SenderId == senderId);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.Include(m => m.Recipient)
                                          .ThenInclude(u => u.MainPhoto)
                                          .Include(m => m.Sender)
                                          .ThenInclude(u => u.MainPhoto)
                                          .FirstOrDefaultAsync(m => m.Id == id);
        }

        // public IEnumerable<Message> GetLatestReceivedMessages(int userId)
        // {
        //     var queryGroup = _context.Messages.Include(m => m.Recipient)
        //                                   .Include(m => m.Sender)
        //                                   .Where(m => m.RecipientId == userId)
        //                                   .GroupBy(m => m.SenderId).ToList();

        //     return queryGroup.Select(g => g.OrderByDescending(m => m.DateCreated).First()).ToList();

        //     // return await _context.Messages.Where(m => m.RecipientId == userId)
        //     //                               .Include(m => m.Recipient)
        //     //                               .Include(m => m.Sender)
        //     //                               .GroupBy(m => m.SenderId)
        //     //                           //    .Select(g => g.OrderByDescending(m => m.DateCreated).First())
        //     //                               .Select(m => m.First())
        //     //                               .ToListAsync();
        // }

        public async Task<PagedList<Message>> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId)
        {
            var messages = _context.Messages.Include(m => m.Recipient)
                                          .Include(m => m.Sender)
                                          .ThenInclude(u => u.MainPhoto)
                                          .Where(m => m.RecipientId == userId && m.SenderId == senderId)
                                          .OrderByDescending(m => m.DateCreated);

            return await PagedList<Message>.CreateAsync(messages, paginationParams.PageNumber, paginationParams.PageSize);

        }

        // public async Task<IEnumerable<Message>> GetLatestSentMessages(int userId)
        // {
        //     return await _context.Messages.Include(m => m.Recipient)
        //                                   .Include(m => m.Sender)
        //                                   .Where(m => m.SenderId == userId)
        //                                   .GroupBy(m => m.RecipientId)
        //                                   .Select(g => g.OrderByDescending(m => m.DateCreated).First())
        //                                   .ToListAsync();
        // }

        public async Task<PagedList<Message>> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId)
        {
            var messages = _context.Messages.Include(m => m.Recipient)
                                            .ThenInclude(u => u.MainPhoto)
                                          .Include(m => m.Sender)
                                          .Where(m => m.SenderId == userId && m.RecipientId == recipientId)
                                          .OrderByDescending(m => m.DateCreated);
            return await PagedList<Message>.CreateAsync(messages, paginationParams.pageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<Message>> GetReceivedMessages(PaginationParams paginationParams, int userId)
        {
            var messages = _context.Messages.Include(m => m.Sender)
                                            .ThenInclude(u => u.MainPhoto)
                                          .Where(m => m.RecipientId == userId)
                                          .OrderByDescending(m => m.DateCreated);
            return await PagedList<Message>.CreateAsync(messages, paginationParams.pageNumber, paginationParams.PageSize);
        }

        public async Task<PagedList<Message>> GetSentMessages(PaginationParams paginationParams, int userId)
        {
            var messages = _context.Messages.Include(m => m.Recipient)
                                          .ThenInclude(u => u.MainPhoto)
                                          .Where(m => m.SenderId == userId)
                                          .OrderByDescending(m => m.DateCreated);

            return await PagedList<Message>.CreateAsync(messages, paginationParams.pageNumber, paginationParams.PageSize);
        }

        public async Task<int> GetNewMessagesCount(int userId)
        {
            return await _context.Messages.Where(m => m.RecipientId == userId && m.IsRead == false).CountAsync();
        }

        // public async Task<Notification> GetNotification(int id){
        //     return await _context.Notifications.Where(n => n.Id == id).FirstOrDefaultAsync();
        // }

        // public async Task<IEnumerable<Notification>> GetNotifications(int appUserId){
        //     return await _context.Notifications.Where(n => n.AppUserId == appUserId).Take(100).ToListAsync();
        // }

        // public void DeleteAllNotifications(int appUserId){
        //     var deletingNotifications = _context.Notifications.Where(n => n.AppUserId == appUserId);
        //     _context.RemoveRange(deletingNotifications);
        // }

        // private async Task AddNotificationIfNotExist(Notification notification){
        //     if(await _context.Notifications.Where(n => 
        //         n.AppUserId == notification.AppUserId
        //         && n.NotificationType == notification.NotificationType
        //         && n.RecordId == notification.RecordId).AnyAsync())
        //         return;

        //     Add(notification);
        // }
        // public async Task AddNotificationRepliedForTopicComment(TopicComment topicComment){
        //     var previousOwnerReply = await _context.TopicReplies.Where(tr => tr.TopicCommentId == topicComment.Id && tr.AppUserId == topicComment.AppUserId)
        //                                                 .OrderByDescending(tr => tr.DateCreated).FirstOrDefaultAsync();
        //     List<TopicReply> notifyingReplies = null;
        //     if (previousOwnerReply == null)
        //     {
        //         notifyingReplies = await _context.TopicReplies.Where(tr =>
        //                                             tr.TopicCommentId == topicComment.Id
        //                                             ).GroupBy(tr => tr.AppUserId)
        //                                             .Select(g => g.First())
        //                                             .ToListAsync();
        //     }
        //     else
        //     {
        //         notifyingReplies = await _context.TopicReplies.Where(tr =>
        //                                             tr.TopicCommentId == topicComment.Id
        //                                             && tr.DateCreated > previousOwnerReply.DateCreated
        //                                             ).GroupBy(tr => tr.AppUserId)
        //                                             .Select(g => g.First())
        //                                             .ToListAsync();
        //     }

        //     foreach (var reply in notifyingReplies)
        //     {
        //         if (reply.AppUserId != null)
        //             await AddNotificationIfNotExist(new Notification() { 
        //                 AppUserId = (int)reply.AppUserId, 
        //                 NotificationType = NotificationEnum.RepliedOnTopicComment, 
        //                 RecordType = "TopicComment", 
        //                 RecordId = reply.TopicCommentId });
        //     }
        // }
        // public async Task AddNotificationNewPostOnTopicComment(int appUserId, TopicComment topicComment){
        //     await AddNotificationIfNotExist(new Notification() { 
        //         AppUserId = (int)topicComment.AppUserId, 
        //         NotificationType = NotificationEnum.NewPostOnTopicComment, 
        //         RecordType = "TopicComment", 
        //         RecordId = topicComment.Id });
        // }

        // public async Task AddNotificationNewMessage(Message message){
        //     await AddNotificationIfNotExist(new Notification() { 
        //         AppUserId = message.RecipientId, 
        //         NotificationType = NotificationEnum.NewMessage, 
        //         RecordType = "Message", 
        //         RecordId = message.Id });
        // }
    
    }
}