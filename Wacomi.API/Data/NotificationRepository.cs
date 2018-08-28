using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class NotificationRepository : RepositoryBase, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context){}

        public async Task<Notification> GetNotification(int id){
            return await _context.Notifications.Include(n => n.Photo).Where(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotifications(int appUserId){
            return await _context.Notifications.Include(n => n.Photo).Where(n => n.AppUserId == appUserId).Take(100).ToListAsync();
        }

        public void DeleteAllNotifications(int appUserId){
            var deletingNotifications = _context.Notifications.Where(n => n.AppUserId == appUserId);
            _context.RemoveRange(deletingNotifications);
        }

        private async Task AddNotificationIfNotExist(Notification notification){
            if(await _context.Notifications.Where(n => 
                n.AppUserId == notification.AppUserId
                && n.NotificationType == notification.NotificationType
                && n.RecordId == notification.RecordId).AnyAsync())
                return;

            Add(notification);
        }
        public async Task AddNotificationRepliedForTopicComment(TopicComment topicComment){
            var previousOwnerReply = await _context.TopicReplies.Where(tr => tr.TopicCommentId == topicComment.Id && tr.AppUserId == topicComment.AppUserId)
                                                        .OrderByDescending(tr => tr.DateCreated).FirstOrDefaultAsync();
            List<TopicReply> notifyingReplies = null;
            if (previousOwnerReply == null)
            {
                notifyingReplies = await _context.TopicReplies.Where(tr =>
                                                    tr.TopicCommentId == topicComment.Id
                                                    ).GroupBy(tr => tr.AppUserId)
                                                    .Select(g => g.First())
                                                    .ToListAsync();
            }
            else
            {
                notifyingReplies = await _context.TopicReplies.Where(tr =>
                                                    tr.TopicCommentId == topicComment.Id
                                                    && tr.DateCreated > previousOwnerReply.DateCreated
                                                    ).GroupBy(tr => tr.AppUserId)
                                                    .Select(g => g.First())
                                                    .ToListAsync();
            }

            foreach (var reply in notifyingReplies)
            {
                if (reply.AppUserId != null)
                    await AddNotificationIfNotExist(new Notification() { 
                        AppUserId = (int)reply.AppUserId, 
                        NotificationType = NotificationEnum.RepliedOnTopicComment, 
                        RecordType = "TopicComment", 
                        RecordId = reply.TopicCommentId,
                        Photo = topicComment.Photo,
                        Message = "あなたのコメントに返信がありました（一言トピック：" + topicComment.Comment +"）"
                         });
            }
        }
        public async Task AddNotificationNewPostOnTopicComment(int appUserId, TopicComment topicComment){
            await AddNotificationIfNotExist(new Notification() { 
                AppUserId = (int)topicComment.AppUserId, 
                NotificationType = NotificationEnum.NewPostOnTopicComment, 
                RecordType = "TopicComment", 
                RecordId = topicComment.Id,
                Message = "あなたの一言『" + topicComment.Comment + "』に新しいコメントがあります"
                });
        }

        public async Task AddNotificationNewMessage(Message message){
            await AddNotificationIfNotExist(new Notification() { 
                AppUserId = message.RecipientId, 
                NotificationType = NotificationEnum.NewMessage, 
                RecordType = "Message", 
                Photo = message.Sender.MainPhoto,
                Message = message.Sender.DisplayName + " さんからメッセージが届いています",
                RecordId = message.Id });
        }

        public async Task AddNotificationRepliedForFeedComment(BlogFeed blogFeed)
        {
            var previousOwnerReply = await _context.BlogFeedComments.Where(fc => fc.BlogFeedId == blogFeed.Id && fc.AppUserId == blogFeed.Blog.OwnerId)
                                                        .OrderByDescending(tr => tr.DateCreated).FirstOrDefaultAsync();
            List<BlogFeedComment> notifyingReplies = null;
            if (previousOwnerReply == null)
            {
                notifyingReplies = await _context.BlogFeedComments.Where(fc =>
                                                    fc.BlogFeedId == blogFeed.Id
                                                    ).GroupBy(fc => fc.AppUserId)
                                                    .Select(g => g.First())
                                                    .ToListAsync();
            }
            else
            {
                notifyingReplies = await _context.BlogFeedComments.Where(fc =>
                                                    fc.BlogFeedId == blogFeed.Id
                                                    && fc.DateCreated > previousOwnerReply.DateCreated
                                                    ).GroupBy(fc => fc.AppUserId)
                                                    .Select(g => g.First())
                                                    .ToListAsync();
            }

            foreach (var reply in notifyingReplies)
            {
                if (reply.AppUserId != null)
                    await AddNotificationIfNotExist(new Notification() { 
                        AppUserId = (int)reply.AppUserId, 
                        NotificationType = NotificationEnum.RepliedOnFeedComment, 
                        RecordType = "BlogFeed", 
                        RecordId = (int)reply.BlogFeedId,
                        Photo = blogFeed.Photo,
                        Message = "あなたがコメントしたブログフィード『" + blogFeed.Title + "』に返信があります"
                         });
            }
        }

        public async Task AddNotificationNewPostOnFeedComment(int appUserId, BlogFeed blogFeed)
        {
            await AddNotificationIfNotExist(new Notification() { 
                AppUserId = blogFeed.Blog.OwnerId, 
                NotificationType = NotificationEnum.NewPostOnFeedComment, 
                RecordType = "BlogFeed", 
                RecordId = blogFeed.Id,
                Message = "あなたのブログフィード『" + blogFeed.Title + "』に新しいコメントがあります" });
        }
    }
}