using System;
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
         Task<int> SaveAll();
         Task<bool> RecordExist(string recordType, int id);
        // Task<T> GetRecord<T>(string recordType, int id) where T: class;

         Task<IEnumerable<City>> GetCities();
         Task<IEnumerable<HomeTown>> GetHomeTowns();
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetPhotoByPublicId(string publicId);
        //  Task<IEnumerable<Photo>> GetPhotosForClass(string className, int id);
        Task<IEnumerable<Photo>> GetPhotosForAppUser(int id);
        Task<IEnumerable<Photo>> GetPhotosForRecord(string recordType, int recordId);
         Task<AppUser> GetAppUser(int id);
         Task<AppUser> GetAppUserByAccountId(string accountId);
         Task AddLikeCountToUser(int userId);

        // Task<UserBase> GetUser(string type, int id);
        Task<BusinessProfile> GetBusinessProfile(int id);
        Task<MemberProfile> GetMemberProfile(int id);
        Task<MemberProfile> GetMemberProfileByAccountId(string id);
        // Task<IEnumerable<MemberProfile>> GetMemberProfiles(UserParams userParams)
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
        Task<IEnumerable<BlogFeed>> GetBlogFeeds(DateTime? from, DateTime? to);
        Task<IEnumerable<BlogFeed>> GetBlogFeedsByBlogId(int blogId);
        Task<int> GetBlogFeedsCountForBlog(int blogId);
        Task DeleteFeed(BlogFeed feed);
        Task DeleteFeeds(System.DateTime? targetDate = null);
        Task DeleteFeedLikes(int feedId);
        Task DeleteFeedComments(int feedId);
        Task DeleteFeedsForBlog(int blogId);

        Task<ClanSeek> GetClanSeek(int id);
        Task<IEnumerable<ClanSeekCategory>> GetClanSeekCategories();
        //Task<IEnumerable<ClanSeek>> GetClanSeeks(int? categoryId, int? cityId, bool? latest);
        Task<PagedList<ClanSeek>> GetClanSeeks(PaginationParams paginationParams, int? categoryId = null, int? cityId = null);
        Task<IEnumerable<ClanSeek>> GetClanSeeksByUser(int userId);
        Task<int> GetClanSeeksCountByUser(int userId);
        Task<PropertySeek> GetPropertySeek(int id);
        Task<IEnumerable<PropertySeek>> GetPropertySeeks(int? categoryId);
        Task<IEnumerable<PropertySeekCategory>> GetPropertySeekCategories();

        Task<DailyTopic> GetDailyTopic(int id); 

        Task<DailyTopic> GetActiveDailyTopic();
        Task<DailyTopic> GetTopDailyTopic();
        Task<string> GetTodaysTopic();
        Task<DailyTopic> GetOldestDailyTopic();
        Task<int> GetPostedTopicCountForUser(int userId);
        Task<TopicLike> GetTopicLike(int userId, int recordId);
        Task<IEnumerable<DailyTopic>> GetDailyTopicList();
        Task<IEnumerable<TopicLike>> GetTopicLikesForUser(int userId);
        Task<IEnumerable<TopicLike>> GetTopicLikesForTopic(int topicId);
        void ResetTopicLikes(int topicId);

        Task<TopicComment> GetTopicComment(int id);
        Task<IEnumerable<TopicComment>> GetTopicCommentsForMember(int userId);
        Task<IEnumerable<TopicComment>> GetLatestTopicCommentList();
        Task<IEnumerable<TopicComment>> GetTopicComments();
        void ResetTopicComments();

        Task<TopicReply> GetTopicReply(int id);
        Task<IEnumerable<TopicReply>> GetTopicRepliesByCommentId(int commentId);

        Task<TopicCommentFeel> GetCommentFeel(int userId, int commentId);
        Task<IEnumerable<TopicCommentFeel>> GetCommentFeels(int userId);

        Task<Friend> GetFriend(int memberId, int friendId);
        Task<IEnumerable<Friend>> GetFriends(int memberId);
        Task<FriendRequest> GetFriendRequestFrom(int memberId, int senderId);
        Task<FriendRequest> GetFriendRequest(int senderId, int recipientId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsReceived(int memberId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsSent(int memberId);

        Task<Message> GetMessage(int id);
        Task<PagedList<Message>> GetReceivedMessages(PaginationParams paginationParams,int userId);
        // IEnumerable<Message> GetLatestReceivedMessages(int userId);
//        Task<IEnumerable<Message>> GetReceivedMessagesFrom(int userId, int senderId);
        Task<PagedList<Message>> GetReceivedMessagesFrom(PaginationParams paginationParams, int userId, int senderId);
        Task<PagedList<Message>> GetSentMessages(PaginationParams paginationParams, int userId);
        // Task<IEnumerable<Message>> GetLatestSentMessages(int userId);
        Task<PagedList<Message>> GetMessagesSentTo(PaginationParams paginationParams, int userId, int recipientId);
        Task<int> GetNewMessagesCount(int userId);
        // Task<Notification> GetNotification(int id);
        // Task<IEnumerable<Notification>> GetNotifications(int appUserId);
        // void DeleteAllNotifications(int appUserId);
        // Task AddNotificationNewMessage(Message message);
        // Task AddNotificationRepliedForTopicComment(TopicComment topicComment);
        // Task AddNotificationNewPostOnTopicComment(int appUserId, TopicComment topicComment);

    }

    
}