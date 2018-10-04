using System.Threading.Tasks;
using Integration;
using Xunit;

namespace Wacomi.Integration.tests
{
    public class CircleControllerTest
    {
        private readonly TestContext _sut;

        public CircleControllerTest()
        {
            _sut = new TestContext();
        }

        [Fact]
        public async Task GetReturnsOkResponse()
        {
            var response = await _sut.Client.GetAsync("/api/circle/categories");

            response.EnsureSuccessStatusCode();
            Assert.Equal("Test", await response.Content.ReadAsStringAsync());
        }
    }
}