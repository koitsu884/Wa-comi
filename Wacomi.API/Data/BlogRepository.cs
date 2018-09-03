using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class BlogRepository : RepositoryBase, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
        }

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
                                                  .Where(bfc => bfc.BlogFeedId == feedId)
                                                  .OrderBy(bfc => bfc.DateCreated)
                                                  .ToListAsync();
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

        public async Task<IEnumerable<BlogFeed>> GetBlogFeeds(System.DateTime? from = null, System.DateTime? to = null)
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
        public async Task DeleteFeeds(System.DateTime? targetDate = null)
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
    }
}