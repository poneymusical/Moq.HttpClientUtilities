# HttpClient.TestUtilities

![Build Status](https://dev.azure.com/guhke/Moq.HttpClientUtilities/_apis/build/status/poneymusical.Moq.HttpClientUtilities?branchName=main "Build Status")

This nuget package helps you building unit tests for classes that use HttpClient/IHttpClientFactory.

It is required that you use Moq for mocking your dependencies in your unit tests.

## What is it?

This package contains extension methods on

## Installation

Just grab the [nuget package](https://www.nuget.org/packages/HttpClient.TestUtilities/).

## Usage

Let's say you need to test this class:

```csharp
using System.Net.Http;
using System.Threading.Tasks;

namespace Moq.HttpClientUtilities.Samples
{
    public class MyService
    {
        private readonly HttpClient _httpClient;

        internal const string Path = "path";

        public MyService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetValue() => await _httpClient.GetStringAsync(Path);
    }
}
```

You cannot mock HttpClient directly. You have to create a mock HttpMessageHandler, which can be a hassle. With HttpClient.TestUtilities, you can write tests as easy as:

```csharp
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

        private MyService BuildService() =>
            new(_mockHttpMessageHandler.CreateHttpClientFactory("http://unittest"));
    }
}
```

Several extension methods are available to tailor mocked responses to your needs. You can combine multiple response setups on the same mock instance: the same rules as standard setups with Moq apply.

You can pass a `IList<HttpRequestMessage>` to each of your setups. Every request that falls under a setup where you passed a list will be added to that list, so that you can check them afterwards. Example:

```csharp
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
```