using System.Threading.Tasks;
using Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Integration
{
    public class BlogFeedControllerTest
    {
        private readonly TestContext _sut;

        public BlogFeedControllerTest()
        {
            _sut = new TestContext();
        }

        [Fact]
        public async Task GetReturnsOkResponse()
        {
            var response = await _sut.Client.GetAsync("/api/blogfeed/test");
            Assert.Equal("Test", await response.Content.ReadAsStringAsync());
        }
    }
}