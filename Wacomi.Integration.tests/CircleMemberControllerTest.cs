using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wacomi.API.Dto;
using Wacomi.API.Models;
using Wacomi.Integration.tests.Helper;
using Xunit;

namespace Wacomi.Integration.tests
{
    public class CircleMemberControllerTest : TestContext
    {

        [Fact]
        public async Task AcceptRequest_AddMemberAndNotification()
        {
            //Arrange
            string circleOwnerName = "TestMember1";
            // int ownerUserId = TestData.GetAppUserIdByName(circleOwnerName);
            // int ownerUserId = 11;
            string requestUserName = "TestMember2";
            // int requestUserId = 13;
            int requestUserId = TestDataSeeder.GetAppUserIdByName(requestUserName);

            //Act
            var approvingRequest = new CircleRequest{AppUserId = requestUserId, CircleId = 1};

            //Accept circle request by owner
            await LoginByUser(circleOwnerName);
            var contents = new StringContent(JsonConvert.SerializeObject(approvingRequest), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/circlemember/approve", contents);
            response.EnsureSuccessStatusCode();
            //Check if a new member was added
            response = await _client.GetAsync("/api/circlemember/" + approvingRequest.CircleId + "/latest");
            response.EnsureSuccessStatusCode();
            var jsonResult = JsonConvert.DeserializeObject<JArray>(await response.Content.ReadAsStringAsync());
            var memberList = jsonResult.ToObject<List<CircleMember>>();
            Assert.True(memberList.Any(ml => ml.AppUserId == requestUserId && ml.CircleId == 1));

            //Check if notification was created
            await LoginByUser(requestUserName);
            var notifications = await _client.GetAsync("/api/notification/" + requestUserId);
            string result = await notifications.Content.ReadAsStringAsync();
            Assert.Contains("notificationType\":" + (int)NotificationEnum.CircleRequestAccepted, result);
        }

    }
}