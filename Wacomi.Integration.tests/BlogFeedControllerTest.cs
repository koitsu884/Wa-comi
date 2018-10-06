using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Integration
{
    public class BlogFeedControllerTest : TestContext
    {

        [Fact]
        public async Task GetReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/blogfeed/test");
            response.EnsureSuccessStatusCode();
            // Assert.Equal("Test", await response.Content.ReadAsStringAsync());
        }
    }
}