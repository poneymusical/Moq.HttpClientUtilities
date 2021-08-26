using System;
using System.Net.Http;

namespace Moq.HttpClientUtilities.Utils
{
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Checks if the HttpRequestMessage object matches the specified path and query.
        /// </summary>
        /// <param name="pathAndQuery">The path and query</param>
        /// <returns>true if it matches, false otherwise.</returns>
        public static bool MatchesUri(this HttpRequestMessage request, string pathAndQuery)
        {
            var fixedUri = pathAndQuery.StartsWith("/") ? pathAndQuery : pathAndQuery.Insert(0, "/");
            return request.RequestUri.PathAndQuery.Equals(fixedUri, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}