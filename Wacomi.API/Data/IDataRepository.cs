using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Helper;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IDataRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         void DeleteAll<T>(T entities) where T: class;
         Task<bool> SaveAll();

         Task<IEnumerable<City>> GetCities();
         Task<IEnumerable<HomeTown>> GetHomeTowns();
         Task<Photo> GetPhoto(int id);
         Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id);

        Task<UserBase> GetUser(string type, int id);
        Task<BusinessUser> GetBusinessUser(int id);
        Task<Member> GetMember(int id);
        Task<Member> GetMemberByIdentityId(string id);
        Task<IEnumerable<Member>> GetMembers(UserParams userParams);
        Task<bool> MemberExist(int memberId);
        Task<Blog> GetBlog(int id);
        Task<IEnumerable<Blog>> GetBlogsForClass(string className, int id);
        Task<IEnumerable<Blog>> GetBlogs();
        Task<BlogFeed> GetLatestBlogFeed(Blog blog);
        Task<BlogFeed> GetBlogFeed(int id);
        Task<IEnumerable<BlogFeed>> GetLatestBlogFeeds();
        Task<ClanSeek> GetClanSeek(int id);
        Task<IEnumerable<ClanSeekCategory>> GetClanSeekCategories();
        Task<IEnumerable<ClanSeek>> GetClanSeeks(int? categoryId, int? cityId, bool? latest);
        Task<PropertySeek> GetPropertySeek(int id);
        Task<IEnumerable<PropertySeek>> GetPropertySeeks(int? categoryId);
        Task<IEnumerable<PropertySeekCategory>> GetPropertySeekCategories();

        Task<DailyTopic> GetDailyTopic(int id);
        Task<DailyTopic> GetActiveDailyTopic();
        Task<DailyTopic> GetTopDailyTopic();
        Task<string> GetTodaysTopic();
        Task<DailyTopic> GetOldestDailyTopic();
        Task<int> GetPostedTopicCountForUser(string userId);
        Task<TopicLike> GetTopicLike(string userId, int recordId);
        Task<IEnumerable<DailyTopic>> GetDailyTopicList();
        Task<IEnumerable<TopicLike>> GetTopicLikesForUser(string userId);
        Task<IEnumerable<TopicLike>> GetTopicLikesForTopic(int topicId);
        void ResetTopicLikes(int topicId);

        Task<TopicComment> GetTopicComment(int id);
        Task<IEnumerable<TopicComment>> GetTopicCommentsForMember(int memberId);
        Task<IEnumerable<TopicComment>> GetLatestTopicCommentList();
        Task<IEnumerable<TopicComment>> GetTopicComments();
        void ResetTopicComments();

        Task<bool> TopicCommentExists(int id);
        Task<TopicReply> GetTopicReply(int id);
        Task<IEnumerable<TopicReply>> GetTopicRepliesByCommentId(int commentId);

        Task<TopicCommentFeel> GetCommentFeel(int memberId, int commentId);
        Task<IEnumerable<TopicCommentFeel>> GetCommentFeels(int memberId);

        Task<Friend> GetFriend(int memberId, int friendId);
        Task<IEnumerable<Friend>> GetFriends(int memberId);
        Task<FriendRequest> GetFriendRequestFrom(int memberId, int senderId);
        Task<FriendRequest> GetFriendRequest(int senderId, int recipientId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsReceived(int memberId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int memberId);

    }

    
}