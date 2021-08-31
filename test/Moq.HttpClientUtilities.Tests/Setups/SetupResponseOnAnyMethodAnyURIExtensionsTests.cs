using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Moq.HttpClientUtilities.Tests.Setups
{
    public class SetupResponseOnAnyMethodAnyURIExtensionsTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<HttpMessageHandler> _handler = new();

        [Theory]
        [MemberData(nameof(StatusCodesAndMethods))]
        public async Task SetupResponse_HttpContent_SentRequestsIsNull(HttpStatusCode statusCode, HttpMethod method)
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            List<HttpRequestMessage> sentRequests = null;

            _handler.SetupResponseOnAnyMethodAnyURI(statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();
            var response = await client.SendAsync(new HttpRequestMessage(method, path));

            sentRequests.Should().BeNull();
            response.StatusCode.Should().Be(statusCode);
            response.Content.Should().Be(content);
        }

        [Theory]
        [MemberData(nameof(StatusCodesAndMethods))]
        public async Task SetupResponse_HttpContent(HttpStatusCode statusCode, HttpMethod method)
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnAnyMethodAnyURI(statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            TestUtils.CheckResult(sentRequests, request, response, statusCode, content);
        }

        [Theory]
        [MemberData(nameof(StatusCodesAndMethods))]
        public async Task SetupResponse_String(HttpStatusCode statusCode, HttpMethod method)
        {
            var content = _fixture.Create<string>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnAnyMethodAnyURI(statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, statusCode, content);
        }

        [Theory]
        [MemberData(nameof(StatusCodesAndMethods))]
        public async Task SetupResponse_ByteArray(HttpStatusCode statusCode, HttpMethod method)
        {
            var content = _fixture.Create<byte[]>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnAnyMethodAnyURI(statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, statusCode, content);
        }

        [Theory]
        [MemberData(nameof(StatusCodesAndMethods))]
        public async Task SetupResponse_Object(HttpStatusCode statusCode, HttpMethod method)
        {
            var content = new { prop = _fixture.Create<string>() };
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnAnyMethodAnyURI(statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, statusCode, content);
        }

        public static IEnumerable<object[]> StatusCodesAndMethods() =>
            TestUtils.StatusCodesAndMethods();
    }
}