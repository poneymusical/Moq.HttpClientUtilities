using System.Net.Http;
using AutoFixture;
using FluentAssertions;
using Moq.HttpClientUtilities.Utils;
using Xunit;

namespace Moq.HttpClientUtilities.Tests.Utils
{
    public class HttpRequestMessageExtensionsTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void MatchesUri_ReturnsFalse()
        {
            var path = _fixture.Create<string>();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://unittest/{path}");
            request.MatchesUri(_fixture.Create<string>()).Should().BeFalse();
            request.MatchesUri($"/{_fixture.Create<string>()}").Should().BeFalse();
        }

        [Fact]
        public void MatchesUri_ReturnsTrue()
        {
            var path = _fixture.Create<string>();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://unittest/{path}");
            request.MatchesUri(path).Should().BeTrue();
            request.MatchesUri($"/{path}").Should().BeTrue();
        }
    }
}