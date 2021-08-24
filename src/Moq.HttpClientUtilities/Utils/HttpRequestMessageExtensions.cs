using System;
using System.Net.Http;

namespace Moq.HttpClientUtilities.Utils
{
    internal static class HttpRequestMessageExtensions
    {
        public static bool MatchesUri(this HttpRequestMessage request, string uri)
        {
            var fixedUri = uri.StartsWith("/") ? uri : uri.Insert(0, "/");
            return request.RequestUri.PathAndQuery.Equals(fixedUri, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}