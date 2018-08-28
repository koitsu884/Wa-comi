using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface INotificationRepository : IRepositoryBase
    {
         Task<Notification> GetNotification(int id);
        Task<IEnumerable<Notification>> GetNotifications(int appUserId);
        void DeleteAllNotifications(int appUserId);
        Task AddNotificationNewMessage(Message message);
        Task AddNotificationRepliedForTopicComment(TopicComment topicComment);
        Task AddNotificationNewPostOnTopicComment(int appUserId, TopicComment topicComment);
        Task AddNotificationRepliedForFeedComment(BlogFeed blogFeed);
        Task AddNotificationNewPostOnFeedComment(int appUserId, BlogFeed blogFeed);
    }
}