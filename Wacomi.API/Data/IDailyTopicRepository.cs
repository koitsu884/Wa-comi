using System.Collections.Generic;
using System.Threading.Tasks;
using Wacomi.API.Models;

namespace Wacomi.API.Data
{
    public interface IDailyTopicRepository :IRepositoryBase
    {
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
    }
}