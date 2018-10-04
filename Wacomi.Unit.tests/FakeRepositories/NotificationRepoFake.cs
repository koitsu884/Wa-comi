using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wacomi.API.Data;
using Wacomi.API.Models;

namespace Wacomi.Xunit.FakeRepositories
{
    public class NotificationRepoFake : RepositoryBase, INotificationRepository
    {
        private readonly List<Notification> _notificationList;
        public NotificationRepoFake(ApplicationDbContext context) : base(context)
        {
            this._notificationList = new List<Notification>();
        }

        public async Task AddNotification(NotificationEnum type, int appUserId, object record)
        {
            switch (type)
            {
                case NotificationEnum.NewCircleMemberRequest:
                    var request = record as CircleRequest;
                    this._notificationList.Add(new Notification()
                    {
                        AppUserId = (int)request.AppUserId,
                        NotificationType = NotificationEnum.NewCircleMemberRequest,
                        RecordType = "Circle",
                        RecordId = (int)request.CircleId,
                        // Photo = blogFeed.Photo,
                        Message = "New circle request for circle " + request.CircleId
                    });
                    break;
                case NotificationEnum.CircleRequestAccepted:
                    var circleMember = record as CircleMember;
                    this._notificationList.Add(new Notification()
                    {
                        AppUserId = (int)circleMember.AppUserId,
                        NotificationType = NotificationEnum.CircleRequestAccepted,
                        RecordType = "Circle",
                        RecordId = (int)circleMember.CircleId,
                        // Photo = blogFeed.Photo,
                        Message = "Circle request was accepted for circle " + circleMember.CircleId
                    });
                    break;
                // case NotificationEnum.NewCircleTopicCreated:
                //     await this.AddNewCircleTopicCreatedNotification(appUserId, record as CircleTopic);
                //     break;
                // case NotificationEnum.NewCircleCommentReplyByOwner:
                //     await this.AddNewCircleCommentReplyByOwnerNotification(appUserId, record as CircleTopicCommentReply);
                //     break;
                // case NotificationEnum.NewCircleCommentReplyByMember:
                //     await this.AddNewCircleCommentReplyByMemberNotification(appUserId, record as CircleTopicCommentReply);
                //     break;

            }
            return;
        }

        public Task AddNotificationNewMessage(Message message)
        {
            throw new System.NotImplementedException();
        }

        public Task AddNotificationNewPostOnFeedComment(int appUserId, BlogFeed blogFeed)
        {
            throw new System.NotImplementedException();
        }

        public Task AddNotificationNewPostOnTopicComment(int appUserId, TopicComment topicComment)
        {
            throw new System.NotImplementedException();
        }

        public Task AddNotificationRepliedForFeedComment(BlogFeed blogFeed)
        {
            throw new System.NotImplementedException();
        }

        public Task AddNotificationRepliedForTopicComment(TopicComment topicComment)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteAllNotifications(int appUserId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Notification> GetNotification(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetNotifications(int appUserId)
        {
            return Task.FromResult(this._notificationList.Where(nl =>nl.AppUserId == appUserId));
        }
    }
}