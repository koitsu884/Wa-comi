using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Integration;
using Newtonsoft.Json;
using Wacomi.API.Models;
using Wacomi.Integration.tests.Helper;
using Xunit;

namespace Wacomi.Integration.tests
{
    public class CircleTopicCommentReplyControllerTest  : TestContext
    {
        [Fact]
        public async Task AddCircleTopicReply_AddNotificationToCommentOwner()
        {
            var repliedUserName = "TestMember3";
            int repliedUserNameId = TestDataSeeder.GetAppUserIdByName(repliedUserName);
            var commentOwnerName = "TestMember1";
            int commentOwnerId = TestDataSeeder.GetAppUserIdByName(commentOwnerName);
            
            await LoginByUser(repliedUserName);
            var contents = BuildStringContent(new CircleTopicCommentReply(){AppUserId = repliedUserNameId, CommentId = 1, Reply = "Reply Test"});
            var response = await _client.PostAsync("/api/circletopiccommentreply", contents);
            response.EnsureSuccessStatusCode();

            //Check if notification was created
            await LoginByUser(commentOwnerName);
            var notifications = await _client.GetAsync("/api/notification/" + commentOwnerId);
            string result = await notifications.Content.ReadAsStringAsync();
            Assert.Contains("notificationType\":" + (int)NotificationEnum.NewCircleCommentReplyByMember, result);
        }

        [Fact]
        public async Task AddCircleTopicReplyByOwner_AddNotificationToWhomeCommented()
        {
            var commentOwnerName = "TestMember1";
            int commentOwnerId = TestDataSeeder.GetAppUserIdByName(commentOwnerName);
            var repliedUserName = "TestMember3";
            int repliedUserNameId = TestDataSeeder.GetAppUserIdByName(repliedUserName);
            var repliedUserName2 = "TestMember4";
            int repliedUserNameId2 = TestDataSeeder.GetAppUserIdByName(repliedUserName2);

            //Added comment by member 1
            await LoginByUser(repliedUserName);
            var contents = BuildStringContent(new CircleTopicCommentReply(){AppUserId = repliedUserNameId, CommentId = 1, Reply = "Reply Test"});
            var response = await _client.PostAsync("/api/circletopiccommentreply", contents);
            response.EnsureSuccessStatusCode();
            //Added comment by member 2
            await LoginByUser(repliedUserName2);
            contents = BuildStringContent(new CircleTopicCommentReply(){AppUserId = repliedUserNameId2, CommentId = 1, Reply = "Reply Test 2"});
            response = await _client.PostAsync("/api/circletopiccommentreply", contents);
            response.EnsureSuccessStatusCode();

            //Added comment by comment owner
            await LoginByUser(commentOwnerName);
            contents = BuildStringContent(new CircleTopicCommentReply(){AppUserId = commentOwnerId, CommentId = 1, Reply = "Reply by owner"});
            response = await _client.PostAsync("/api/circletopiccommentreply", contents);
            response.EnsureSuccessStatusCode();

            //Check if notification was created
            await LoginByUser(repliedUserName);
            var notifications = await _client.GetAsync("/api/notification/" + repliedUserNameId);
            string result = await notifications.Content.ReadAsStringAsync();
            Assert.Contains("notificationType\":" + (int)NotificationEnum.NewCircleCommentReplyByOwner, result);

             await LoginByUser(repliedUserName2);
            notifications = await _client.GetAsync("/api/notification/" + repliedUserNameId2);
            result = await notifications.Content.ReadAsStringAsync();
            Assert.Contains("notificationType\":" + (int)NotificationEnum.NewCircleCommentReplyByOwner, result);
        }
    }
}