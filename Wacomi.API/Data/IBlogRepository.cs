using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IBlogRepository : IRepositoryBase
    {
          Task<Blog> GetBlog(int id);
        Task<IEnumerable<Blog>> GetBlogs();
        Task<IEnumerable<Blog>> GetBlogsForRssFeed(int count = 100);
        Task<int> GetBlogCountForUser(int id);
        Task<IEnumerable<Blog>> GetBlogsForUser(int id);
        // Task<IEnumerable<Blog>> GetBlogsForClass(string className, int id);
        Task<BlogFeed> GetLatestBlogFeed(int blogId);
        Task<BlogFeed> GetBlogFeed(int id);
        Task<bool> BlogFeedLiked(int appUserId, int blogFeedId);
        Task<BlogFeedLike> GetBlogFeedLike(int id);
        Task<IEnumerable<BlogFeedLike>> GetBlogFeedLikesForUser(int userId);
        Task<BlogFeedComment> GetBlogFeedComment(int id);
        Task<IEnumerable<BlogFeedComment>> GetBlogFeedCommentsForFeed(int feedId);
        Task<IEnumerable<BlogFeed>> GetLatestBlogFeeds();
        Task<PagedList<BlogFeed>> GetBlogFeeds(PaginationParams paginationParams, string category, int? userId);
        Task<IEnumerable<BlogFeed>> GetBlogFeeds(System.DateTime? from, System.DateTime? to);
        Task<IEnumerable<BlogFeed>> GetBlogFeedsByBlogId(int blogId);
        Task<int> GetBlogFeedsCountForBlog(int blogId);
        Task DeleteFeed(BlogFeed feed);
        Task DeleteFeeds(System.DateTime? targetDate = null);
        Task DeleteFeedLikes(int feedId);
        Task DeleteFeedComments(int feedId);
        Task DeleteFeedsForBlog(int blogId);
    }
}