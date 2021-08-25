using System.Net.Http;
using System.Threading;
using Moq.HttpClientUtilities.Utils;
using Moq.Protected;

namespace Moq.HttpClientUtilities
{
    public static class VerifyExtensions
    {
        public static void Verify(this Mock<HttpMessageHandler> handler,
            HttpMethod method, string uri, Times times)
        {
            handler.Protected().Verify(SetupExtensions.SendAsync, times, 
                ItExpr.Is<HttpRequestMessage>(x => x.Method == method && x.MatchesUri(uri)), 
                ItExpr.IsAny<CancellationToken>());
        } 
    }
}