using System;
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
    public class SetupResponseOnGETExtensionsTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<HttpMessageHandler> _handler = new();

        [Theory]
        [MemberData(nameof(StatusCodes))]
        public async Task SetupResponseOnGET_HttpContent_SentRequestsIsNull(HttpStatusCode statusCode)
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            List<HttpRequestMessage> sentRequests = null;

            _handler.SetupResponseOnGET(path, statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();
            var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, path));

            sentRequests.Should().BeNull();
            response.StatusCode.Should().Be(statusCode);
            response.Content.Should().Be(content);
        }

        [Theory]
        [MemberData(nameof(StatusCodes))]
        public async Task SetupResponse_HttpContent(HttpStatusCode statusCode)
        {
            var content = new StringContent(_fixture.Create<string>());
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnGET(path, statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            TestUtils.CheckResult(sentRequests, request, response, statusCode, content);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _fixture.Create<string>())));
        }

        [Theory]
        [MemberData(nameof(StatusCodes))]
        public async Task SetupResponse_String(HttpStatusCode statusCode)
        {
            var content = _fixture.Create<string>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnGET(path, statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, statusCode, content);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _fixture.Create<string>())));
        }

        [Theory]
        [MemberData(nameof(StatusCodes))]
        public async Task SetupResponse_ByteArray(HttpStatusCode statusCode)
        {
            var content = _fixture.Create<byte[]>();
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnGET(path, statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, statusCode, content);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _fixture.Create<string>())));
        }

        [Theory]
        [MemberData(nameof(StatusCodes))]
        public async Task SetupResponse_Object(HttpStatusCode statusCode)
        {
            var content = new { prop = _fixture.Create<string>() };
            var path = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();

            _handler.SetupResponseOnGET(path, statusCode, content, sentRequests);

            var client = _handler.CreateHttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, path);
            var response = await client.SendAsync(request);

            await TestUtils.CheckResult(sentRequests, request, response, statusCode, content);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _fixture.Create<string>())));
        }

        public static IEnumerable<object[]> StatusCodes() =>
            TestUtils.StatusCodes();
    }
}