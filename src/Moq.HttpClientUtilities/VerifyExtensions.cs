using System.Net.Http;
using System.Threading;
using Moq.HttpClientUtilities.Utils;
using Moq.Protected;

namespace Moq.HttpClientUtilities
{
    public static class VerifyExtensions
    {
        /// <summary>
        /// Verifies that a HTTP request was sent to the specified path with the specified verb.
        /// Throws an exception if the conditions were not met. 
        /// </summary>
        /// <param name="method">The HTTP method (GET, POST, ...)</param>
        /// <param name="path">The uri fragment (excluding the base path)</param>
        /// <param name="times">How many times the request must have been sent</param>
        public static void Verify(this Mock<HttpMessageHandler> handler,
            HttpMethod method, string path, Times times)
        {
            handler.Protected().Verify(SetupExtensions.SendAsync, times, 
                ItExpr.Is<HttpRequestMessage>(x => x.Method == method && x.MatchesUri(path)), 
                ItExpr.IsAny<CancellationToken>());
        } 
    }
}