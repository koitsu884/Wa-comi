using System.Net.Http;
using System.Threading.Tasks;
using Integration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wacomi.API.Models;
using Wacomi.Integration.tests.Helper;
using Xunit;

namespace Wacomi.Integration.tests
{
    public class CircleEventParticipationControllerTest : TestContext
    {
        [Fact]
        public async Task AddEventParticipant_ConfirmedUntilMaxNumber_WaitingWhenMaxNumber()
        {
            var response = await participateEvent("TestMember4", 1);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("status\":" + (int)CircleEventParticipationStatus.Confirmed, result);

            response = await participateEvent("TestMember5", 1);
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadAsStringAsync();
            Assert.Contains("status\":" + (int)CircleEventParticipationStatus.Confirmed, result);

            response = await participateEvent("TestMember6", 1);
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadAsStringAsync();
            Assert.Contains("status\":" + (int)CircleEventParticipationStatus.Waiting, result);
        }

        [Fact]
        public async Task AddEventParticipant_ConfirmedWhenApproved()
        {
            var response = await participateEvent("TestMember4", 2);
            response.EnsureSuccessStatusCode();

            var jsonResult = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
            var eventParticipation = jsonResult.ToObject<CircleEventParticipation>();
            Assert.Equal(CircleEventParticipationStatus.Waiting, eventParticipation.Status);

            response = await approveEventParticipation("TestMember1", eventParticipation);
            response.EnsureSuccessStatusCode();

            var updatedParticipationGetResponse = await _client.GetAsync($"/api/circleeventparticipation/{eventParticipation.CircleEventId}/{eventParticipation.AppUserId}");
            var result = await updatedParticipationGetResponse.Content.ReadAsStringAsync();
            Assert.Contains("status\":" + (int)CircleEventParticipationStatus.Confirmed, result);
        }

        private async Task<HttpResponseMessage> approveEventParticipation(string eventOwnerName, CircleEventParticipation participation){
            await LoginByUser(eventOwnerName);
            var contents = BuildStringContent(participation);
            return await _client.PutAsync("/api/circleeventparticipation/approve", contents);
        }

        private async Task<HttpResponseMessage> participateEvent(string userName, int circleEventId){
            int userId = TestDataSeeder.GetAppUserIdByName(userName);
            await LoginByUser(userName);
            var contents = BuildStringContent(new CircleEventParticipation(){
                AppUserId = userId, 
                CircleEventId = circleEventId
            });
            return await _client.PostAsync("/api/circleeventparticipation", contents);
        }
    }
}