using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wacomi.API.Dto;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public class NotificationRepository : RepositoryBase, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Notification> GetNotification(int id)
        {
            return await _context.Notifications.Include(n => n.Photo).Where(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotifications(int appUserId)
        {
            return await _context.Notifications.Include(n => n.Photo).Where(n => n.AppUserId == appUserId).Take(100).ToListAsync();
        }

        public void DeleteAllNotifications(int appUserId)
        {
            var deletingNotifications = _context.Notifications.Where(n => n.AppUserId == appUserId);
            _context.RemoveRange(deletingNotifications);
        }

        private async Task AddNotificationIfNotExist(Notification notification)
        {
            if (await _context.Notifications.Where(n =>
                 n.AppUserId == notification.AppUserId
                 && n.NotificationType == notification.NotificationType
                 && n.RecordId == notification.RecordId).AnyAsync())
                return;

            Add(notification);
        }
        public async Task AddNotificationRepliedForTopicComment(TopicComment topicComment)
        {
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

            var commentOwner = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == topicComment.AppUserId);

            foreach (var reply in notifyingReplies)
            {
                if (reply.AppUserId != null)
                    await AddNotificationIfNotExist(new Notification()
                    {
                        AppUserId = (int)reply.AppUserId,
                        NotificationType = NotificationEnum.RepliedOnTopicComment,
                        RecordType = "TopicComment",
                        RecordId = reply.TopicCommentId,
                        Photo = topicComment.Photo,
                        FromUserName = commentOwner.DisplayName,
                        TargetRecordTitle = topicComment.Comment,
                        // Message = "あなたのコメントに返信がありました（一言トピック：" + topicComment.Comment + "）"
                    });
            }
        }
        public async Task AddNotificationNewPostOnTopicComment(int appUserId, TopicComment topicComment)
        {
            var fromUser = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == appUserId);
            await AddNotificationIfNotExist(new Notification()
            {
                AppUserId = (int)topicComment.AppUserId,
                NotificationType = NotificationEnum.NewPostOnTopicComment,
                RecordType = "TopicComment",
                RecordId = topicComment.Id,
                FromUserName = fromUser.DisplayName,
                TargetRecordTitle = topicComment.Comment,
                // Message = "あなたの一言『" + topicComment.Comment + "』に新しいコメントがあります"
            });
        }

        public async Task AddNotificationNewMessage(Message message)
        {
            var sender = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == message.SenderId);
            await AddNotificationIfNotExist(new Notification()
            {
                AppUserId = message.RecipientId,
                NotificationType = NotificationEnum.NewMessage,
                RecordType = "Message",
                RecordId = message.Id,
                Photo = message.Sender.MainPhoto,
                FromUserName = sender.DisplayName
                //Message = message.Sender.DisplayName + " さんからメッセージが届いています",
            });
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

            var blogOwner = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == blogFeed.Blog.OwnerId);

            foreach (var reply in notifyingReplies)
            {
                if (reply.AppUserId != null)
                    await AddNotificationIfNotExist(new Notification()
                    {
                        AppUserId = (int)reply.AppUserId,
                        NotificationType = NotificationEnum.RepliedOnFeedComment,
                        RecordType = "BlogFeed",
                        RecordId = (int)reply.BlogFeedId,
                        Photo = blogFeed.Photo,
                        FromUserName = blogOwner.DisplayName,
                        TargetRecordTitle = blogFeed.Title,
                        //Message = "あなたがコメントしたブログフィード『" + blogFeed.Title + "』に返信があります"
                    });
            }
        }

        public async Task AddNotificationNewPostOnFeedComment(int appUserId, BlogFeed blogFeed)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == appUserId);
            await AddNotificationIfNotExist(new Notification()
            {
                AppUserId = blogFeed.Blog.OwnerId,
                NotificationType = NotificationEnum.NewPostOnFeedComment,
                RecordType = "BlogFeed",
                RecordId = blogFeed.Id,
                FromUserName = user.DisplayName,
                TargetRecordTitle = blogFeed.Title,
                //Message = "あなたのブログフィード『" + blogFeed.Title + "』に新しいコメントがあります"
            });
        }

        public async Task AddNotification(NotificationEnum type, int appUserId, object record)
        {
            switch (type)
            {
                case NotificationEnum.NewCircleMemberRequest:
                    await this.AddNewCircleMemberRequestNotification(appUserId, record as CircleRequest);
                    break;
                case NotificationEnum.CircleRequestAccepted:
                    await this.AddCircleRequestAcceptedNotification(appUserId, record as CircleMember);
                    break;
                case NotificationEnum.NewCircleTopicCreated:
                    await this.AddNewCircleTopicCreatedNotification(appUserId, record as CircleTopic);
                    break;
                case NotificationEnum.NewCircleCommentReplyByOwner:
                    await this.AddNewCircleCommentReplyByOwnerNotification(appUserId, record as CircleTopicCommentReply);
                    break;
                case NotificationEnum.NewCircleCommentReplyByMember:
                    await this.AddNewCircleCommentReplyByMemberNotification(appUserId, record as CircleTopicCommentReply);
                    break;

            }
        }

        private async Task AddNewCircleCommentReplyByOwnerNotification(int appUserId, CircleTopicCommentReply circleTopicCommentReply){
            if(circleTopicCommentReply == null)
                return;
            var circleTopicComment = await _context.CircleTopicComments.Include(ctc => ctc.CircleTopic).Include(ctc => ctc.AppUser).FirstOrDefaultAsync(ctc => ctc.Id == circleTopicCommentReply.CommentId);
            if(circleTopicComment == null)
                return;
            // var circleTopicCommentReply = record as CircleTopicCommentReply;
            var previousOwnerReply = await _context.CircleTopicCommentReplies.Where(
                                                                    ctc => ctc.CommentId == circleTopicCommentReply.CommentId 
                                                                    && ctc.AppUserId == appUserId
                                                                    && ctc.Id != circleTopicCommentReply.Id
                                                                    )
                                                        .OrderByDescending(tr => tr.DateCreated).FirstOrDefaultAsync();
            List<CircleTopicCommentReply> notifyingReplies = null;
            if (previousOwnerReply == null)
            {
                notifyingReplies = await _context.CircleTopicCommentReplies.Where(ctcr =>
                                                    ctcr.CommentId == circleTopicCommentReply.CommentId
                                                     && ctcr.AppUserId != appUserId
                                                    ).GroupBy(fc => fc.AppUserId)
                                                    .Select(g => g.First())
                                                    .ToListAsync();
            }
            else
            {
                notifyingReplies = await _context.CircleTopicCommentReplies.Where(ctcr =>
                                                    ctcr.CommentId == circleTopicCommentReply.CommentId
                                                    && ctcr.DateCreated > previousOwnerReply.DateCreated
                                                    && ctcr.AppUserId != appUserId
                                                    ).GroupBy(fc => fc.AppUserId)
                                                    .Select(g => g.First())
                                                    .ToListAsync();
            }

            foreach (var reply in notifyingReplies)
            {
                Dictionary<string, int> recordIds = new Dictionary<string, int>(){
                    {"Circle", reply.CircleId},
                    {"CircleTopic", circleTopicComment.CircleTopicId}
                };
                if (reply.AppUserId != null)
                    await AddNotificationIfNotExist(new Notification()
                    {
                        AppUserId = (int)reply.AppUserId,
                        NotificationType = NotificationEnum.NewCircleCommentReplyByOwner,
                        RecordType = "CircleTopicComment",
                        RecordId = (int)reply.CommentId,
                        RelatingRecordIds = JObject.FromObject(recordIds),
                        FromUserName = circleTopicComment.AppUser.DisplayName,
                        TargetRecordTitle = circleTopicComment.CircleTopic.Title,
                        // Photo = blogFeed.Photo,
                       // Message = "コミュニティトピック『" + circleTopicComment.CircleTopic.Title +"』であなたが返信した" + circleTopicComment.AppUser.DisplayName + "さんのコメントに、新しい返信がありました"
                    });
            }
        }

        private async Task AddNewCircleCommentReplyByMemberNotification(int appUserId, CircleTopicCommentReply circleTopicCommentReply){
            // var circleTopicComment = record as CircleTopicComment;
            if(circleTopicCommentReply == null)
                return;
            var circleTopicComment = await _context.CircleTopicComments.Include(ctc => ctc.CircleTopic).FirstOrDefaultAsync(ctc => ctc.Id == circleTopicCommentReply.CommentId);
            if(circleTopicComment == null)
                return;
            Dictionary<string, int> recordIds = new Dictionary<string, int>(){
                    {"Circle", circleTopicComment.CircleId},
                    {"CircleTopic", circleTopicComment.CircleTopicId}
            };

            var user = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == appUserId);

            await AddNotificationIfNotExist(new Notification()
            {
                AppUserId = (int)circleTopicComment.AppUserId,
                NotificationType = NotificationEnum.NewCircleCommentReplyByMember,
                RecordType = "CircleTopicComment",
                RecordId = circleTopicComment.Id,
                RelatingRecordIds = JObject.FromObject(recordIds),
                FromUserName = user.DisplayName,
                TargetRecordTitle = circleTopicComment.CircleTopic.Title,
                // AdditionalRecordId = circleTopicComment.CircleTopicId,
               // Message = "コミュニティトピック『" + circleTopicComment.CircleTopic.Title +"』であなたのコメントに返信がありました"
            });
        }
        private async Task AddNewCircleTopicCreatedNotification(int appUserId, CircleTopic circleTopic)
        {
            if(circleTopic == null)
                return;
            Dictionary<string, int> recordIds = new Dictionary<string, int>(){
                    {"Circle", circleTopic.CircleId},
            };

            var circleMembers = await _context.CircleMembers.Include(cm => cm.Circle).Where(cm => cm.CircleId == circleTopic.CircleId && cm.AppUserId != appUserId).ToListAsync();
            foreach(var circleMember in circleMembers){
                await AddNotificationIfNotExist(new Notification()
                {
                    AppUserId = (int)circleMember.AppUserId,
                    NotificationType = NotificationEnum.NewCircleTopicCreated,
                    RecordType = "CircleTopic",
                    RecordId = circleTopic.Id,
                    RelatingRecordIds = JObject.FromObject(recordIds),
                    TargetRecordTitle = circleMember.Circle.Name + '|' + circleTopic.Title,
                 //   Message = "コミュニティ『" + circleMember.Circle.Name + "』に新しいトピック『" + circleTopic.Title + "』が作成されました！"
                });
            }
        }

        private async Task AddCircleRequestAcceptedNotification(int appUserId, CircleMember circleMember)
        {
            // var circleMember = record as CircleMember;
            if(circleMember == null)
                return;

            await AddNotificationIfNotExist(new Notification()
            {
                AppUserId = (int)circleMember.AppUserId,
                NotificationType = NotificationEnum.CircleRequestAccepted,
                RecordType = "Circle",
                RecordId = circleMember.CircleId,
                TargetRecordTitle = circleMember.Circle.Name,
//                Message = "コミュニティ『" + circleMember.Circle.Name + "』への参加が承認されました"
            });
        }

        private async Task AddNewCircleMemberRequestNotification(int appUserId, CircleRequest circleRequest)
        {
            // var circleRequest = record as CircleRequest;
            if(circleRequest == null)
                return;
            var user = await _context.AppUsers.FirstOrDefaultAsync(au => au.Id == appUserId);
            await AddNotificationIfNotExist(new Notification()
            {
                AppUserId = (int)circleRequest.Circle.AppUserId,
                NotificationType = NotificationEnum.NewCircleMemberRequest,
                RecordType = "Circle",
                RecordId = circleRequest.CircleId,
                FromUserName = user.DisplayName,
                TargetRecordTitle = circleRequest.Circle.Name,
               // Message = circleRequest.AppUser.DisplayName + "さんからコミュニティ" + circleRequest.Circle.Name + "の参加リクエストがあります"
            });

        }
    }
}