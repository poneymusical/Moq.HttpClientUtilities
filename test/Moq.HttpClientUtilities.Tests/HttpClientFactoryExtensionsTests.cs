using System;
using System.Net.Http;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace Moq.HttpClientUtilities.Tests
{
    public class HttpClientFactoryExtensionsTests
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<HttpMessageHandler> _handler = new();

        [Fact]
        public void CreateHttpClientFactory_DefaultUri()
        {
            var factory = _handler.CreateHttpClientFactory();
            var expectedClient = _handler.CreateHttpClient();

            var client = factory.CreateClient();
            client.Should().BeEquivalentTo(expectedClient);
            client.BaseAddress.Should().Be("http://unittest");

            var client2 = factory.CreateClient(_fixture.Create<string>());
            client2.Should().BeEquivalentTo(expectedClient);
            client2.BaseAddress.Should().Be("http://unittest");
        }

        [Fact]
        public void CreateHttpClientFactory_String()
        {
            var uri = $"http://{_fixture.Create<string>()}";
            var factory = _handler.CreateHttpClientFactory(uri);
            var expectedClient = _handler.CreateHttpClient(uri);

            var client = factory.CreateClient();
            client.Should().BeEquivalentTo(expectedClient);
            client.BaseAddress.Should().Be(new Uri(uri));

            var client2 = factory.CreateClient(_fixture.Create<string>());
            client2.Should().BeEquivalentTo(expectedClient);
            client2.BaseAddress.Should().Be(new Uri(uri));
        }
    }
}