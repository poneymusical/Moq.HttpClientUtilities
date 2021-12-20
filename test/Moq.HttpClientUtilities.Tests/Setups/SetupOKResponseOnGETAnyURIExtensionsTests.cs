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
    public class SetupOKResponseOnGETAnyURIExtensionsTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<HttpMessageHandler> _handler = new();

        [Fact]
        public async Task SetupResponse_HttpContent_SentRequestsIsNull()
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            List<HttpRequestMessage> sentRequests = null;

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, path));

            sentRequests.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Be(content);
        }

        [Fact]
        public async Task SetupResponse_HttpContent()
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        [Fact]
        public async Task SetupResponse_String()
        {
            var content = _fixture.Create<string>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnAnyMethodAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        [Fact]
        public async Task SetupResponse_ByteArray()
        {
            var content = _fixture.Create<byte[]>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnGETAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }

        [Fact]
        public async Task SetupResponse_Object()
        {
            var content = new { prop = _fixture.Create<string>() };
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupOKResponseOnGETAnyURI(content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, HttpStatusCode.OK, content);
        }
    }
}