using System.Threading.Tasks;
using Integration;
using Xunit;

namespace Wacomi.Integration.tests
{
    public class CircleControllerTest : TestContext
    {
        [Fact]
        public async Task GetReturnsOkResponse()
        {
            var response = await _client.GetAsync("/api/circle/latest");

            response.EnsureSuccessStatusCode();
            // Assert.Equal("Test", await response.Content.ReadAsStringAsync());
        }
    }
}