using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.HttpClientUtilities.Utils;
using Moq.Protected;

namespace Moq.HttpClientUtilities
{
    public static class SetupResponseOnAnyMethodAnyURIExtensions
    {
        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any HTTP method and any path.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, object content,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, statusCode, content.ToJsonString(), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any HTTP method and any path.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, statusCode, new ByteArrayContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any HTTP method and any path.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, statusCode, new StringContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any HTTP method and any path.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, HttpContent content = null, IList<HttpRequestMessage> sentRequests = null)
        {
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(SetupResponseExtensions.SendAsync,
                    ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, _) => { sentRequests?.Add(message); })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }
    }
}