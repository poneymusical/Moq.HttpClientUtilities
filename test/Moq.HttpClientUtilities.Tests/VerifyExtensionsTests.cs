using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq.Protected;
using Xunit;

namespace Moq.HttpClientUtilities.Tests
{
    public class VerifyExtensionsTests
    {
        private readonly Fixture _fixture = new();
        
        [Fact]
        public async Task Verify()
        {
            var path = _fixture.Create<string>();
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(mockHandler.Object){BaseAddress = new Uri("http://unittest")};

            await httpClient.GetAsync(path);
            
            mockHandler.Verify(HttpMethod.Get, path, Times.Once());
            mockHandler.Verify(HttpMethod.Get, _fixture.Create<string>(), Times.Never());
            mockHandler.Verify(HttpMethod.Post, path, Times.Never());
        }
    }
}