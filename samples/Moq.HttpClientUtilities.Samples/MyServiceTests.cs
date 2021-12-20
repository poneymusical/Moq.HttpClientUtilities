using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq.HttpClientUtilities.Setups;
using Moq.HttpClientUtilities.Utils;
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
            
            result.Should().Be(value);
            _mockHttpMessageHandler.Verify(HttpMethod.Get, MyService.Path, Times.Once());
        }

        [Fact]
        public async Task TestGetValue_ShouldThrowIfResponseIsNotFound()
        {
            _mockHttpMessageHandler.SetupResponse(HttpMethod.Get, MyService.Path, HttpStatusCode.NotFound);
            
            Func<Task> action = () => BuildService().GetValue();
            
            await action.Should().ThrowAsync<HttpRequestException>();
            _mockHttpMessageHandler.Verify(HttpMethod.Get, MyService.Path, Times.Once());
        }

        [Fact]
        public async Task TestGetValue_SentRequestsExample()
        {
            var value = _fixture.Create<string>();
            var sentRequests = new List<HttpRequestMessage>();
            _mockHttpMessageHandler.SetupOKResponseOnAnyMethodAnyURI(value, sentRequests);
            
            var result = await BuildService().GetValue();
            
            sentRequests.Should().HaveCount(1);
            sentRequests[0].Method.Should().Be(HttpMethod.Get);
            sentRequests[0].Should().Match<HttpRequestMessage>(request => request.MatchesUri(MyService.Path));
        }

        private MyService BuildService() => 
            new(_mockHttpMessageHandler.CreateHttpClientFactory("http://unittest"));
    }
}