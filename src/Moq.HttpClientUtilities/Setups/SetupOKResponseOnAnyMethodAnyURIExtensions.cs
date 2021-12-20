using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Moq.HttpClientUtilities.Utils;

namespace Moq.HttpClientUtilities.Setups
{
    public static class SetupOKResponseOnAnyMethodAnyURIExtensions
    {
        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupOKResponseOnAnyMethodAnyURI(handler, content.ToJsonString(), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupOKResponseOnAnyMethodAnyURI(handler, new ByteArrayContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupOKResponseOnAnyMethodAnyURI(handler, new StringContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpContent content = null, IList<HttpRequestMessage> sentRequests = null) =>
            handler.SetupResponseOnAnyMethodAnyURI(HttpStatusCode.OK, content, sentRequests);
    }
}