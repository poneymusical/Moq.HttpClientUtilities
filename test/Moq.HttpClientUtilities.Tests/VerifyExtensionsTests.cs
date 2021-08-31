using System.Collections.Generic;
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

        [Theory]
        [MemberData(nameof(StatusCodesAndMethods))]
        public async Task Verify(HttpStatusCode statusCode, HttpMethod method)
        {
            var path = _fixture.Create<string>();
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(statusCode));

            var httpClient = mockHandler.CreateHttpClient();
            await httpClient.SendAsync(new HttpRequestMessage(method, path));

            mockHandler.Verify(method, path, Times.Once());
            mockHandler.Verify(method, _fixture.Create<string>(), Times.Never());
            mockHandler.Verify(TestUtils.DifferentVerb(method), path, Times.Never());
        }

        public static IEnumerable<object[]> StatusCodesAndMethods() =>
            TestUtils.StatusCodesAndMethods();
    }
}