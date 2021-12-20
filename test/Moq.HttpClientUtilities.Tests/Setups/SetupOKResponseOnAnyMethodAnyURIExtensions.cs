using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq.HttpClientUtilities.Setups;
using Xunit;
// ReSharper disable ExpressionIsAlwaysNull

namespace Moq.HttpClientUtilities.Tests.Setups
{
    public class SetupOKResponseOnAnyMethodAnyURIExtensions
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<HttpMessageHandler> _handler = new();

        [Theory]
        [MemberData(nameof(Methods))]
        public async Task SetupResponse_HttpContent_SentRequestsIsNull(HttpMethod method)
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            List<HttpRequestMessage> sentRequests = null;

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();
            var response = await client.SendAsync(new HttpRequestMessage(method, path));

            sentRequests.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Be(content);
        }

        [Theory]
        [MemberData(nameof(Methods))]
        public async Task SetupResponse_HttpContent(HttpMethod method)
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        [Theory]
        [MemberData(nameof(Methods))]
        public async Task SetupResponse_String(HttpMethod method)
        {
            var content = _fixture.Create<string>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        [Theory]
        [MemberData(nameof(Methods))]
        public async Task SetupResponse_ByteArray(HttpMethod method)
        {
            var content = _fixture.Create<byte[]>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        [Theory]
        [MemberData(nameof(Methods))]
        public async Task SetupResponse_Object(HttpMethod method)
        {
            var content = new { prop = _fixture.Create<string>() };
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(method, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        public static IEnumerable<object[]> Methods() =>
            TestUtils.Methods();
    }
}