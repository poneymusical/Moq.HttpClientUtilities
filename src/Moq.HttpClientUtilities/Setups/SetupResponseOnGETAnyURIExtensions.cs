using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Moq.HttpClientUtilities.Utils;

namespace Moq.HttpClientUtilities
{
    public static class SetupResponseOnGETAnyURIExtensions
    {
        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized according to the selected mode.</param>
        /// <param name="mode">The serialization mode.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, object content, Serialization mode = Serialization.Json,
            IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnGETAnyURI(handler, statusCode, content.Serialize(mode), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnGETAnyURI(handler, statusCode, new ByteArrayContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnGETAnyURI(handler, statusCode, new StringContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, HttpContent content = null, IList<HttpRequestMessage> sentRequests = null)
            => handler.SetupResponseOnAnyURI(HttpMethod.Get, statusCode, content, sentRequests);
    }
}