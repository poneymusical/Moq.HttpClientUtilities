using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Moq.HttpClientUtilities.Tests
{
    public class MockHttpMessageHandlerTests
    {
        private const string BaseUri = "http://unittest";

        private readonly Fixture _fixture = new();
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

        [Theory]
        [InlineData("GET", HttpStatusCode.OK)]
        [InlineData("POST", HttpStatusCode.NotFound)]
        public async Task SetupAndVerify(string methodVerb, HttpStatusCode statusCode)
        {
            var method = new HttpMethod(methodVerb);

            var path = _fixture.Create<string>();
            var content = _fixture.Create<StringContent>();
            var sentRequests = new List<HttpRequestMessage>();

            var httpRequest = new HttpRequestMessage(method, path);
            _mockHttpMessageHandler.SetupResponse(method, path, statusCode, content, sentRequests);

            var result = await BuildSUT().SendAsync(httpRequest);

            result.StatusCode.Should().Be(statusCode);
            result.Content.Should().Be(content);
            sentRequests.Should().HaveCount(1);
            sentRequests[0].Should().Be(httpRequest);

            _mockHttpMessageHandler.Verify(method, path, Times.Once());
        }

        private HttpClient BuildSUT() => _mockHttpMessageHandler.CreateHttpClient(BaseUri);
    }
}