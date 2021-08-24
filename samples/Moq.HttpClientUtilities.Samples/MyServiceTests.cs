using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Xunit;

namespace Moq.HttpClientUtilities.Samples
{
    public class MyServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task TestGetValue_ShouldReturnValueIfResponseIsOK()
        {
            var value = _fixture.Create<string>();
            _mockHttpMessageHandler.SetupResponse(HttpMethod.Get, MyService.Path, HttpStatusCode.OK, new StringContent(value));
            var result = await BuildService().GetValue();
            Assert.Equal(value, result);
            _mockHttpMessageHandler.Verify(HttpMethod.Get, MyService.Path, Times.Once());
        }

        [Fact]
        public async Task TestGetValue_ShouldThrowIfResponseIsNotFound()
        {
            _mockHttpMessageHandler.SetupResponse(HttpMethod.Get, MyService.Path, HttpStatusCode.NotFound);
            var service = BuildService();
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetValue());
            _mockHttpMessageHandler.Verify(HttpMethod.Get, MyService.Path, Times.Once());
        }

        private MyService BuildService() => 
            new(_mockHttpMessageHandler.CreateHttpClientFactory("http://unittest"));
    }
}