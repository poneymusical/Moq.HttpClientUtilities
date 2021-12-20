using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.HttpClientUtilities.Utils;
using Moq.Protected;

namespace Moq.HttpClientUtilities.Setups
{
    public static class SetupResponseOnAnyURIExtensions
    {
        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any request that matches the HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> handler,
            HttpMethod method, HttpStatusCode statusCode, object content,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, method, statusCode, content.ToJsonString(), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any request that matches the HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> handler,
            HttpMethod method, HttpStatusCode statusCode, byte[] content,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, method, statusCode, new ByteArrayContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any request that matches the HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> handler,
            HttpMethod method, HttpStatusCode statusCode, string content,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, method, statusCode, new StringContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any request that matches the HTTP method.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> handler,
            HttpMethod method, HttpStatusCode statusCode, HttpContent content = null,
            IList<HttpRequestMessage> sentRequests = null)
        {
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(SetupResponseExtensions.SendAsync,
                    ItExpr.Is<HttpRequestMessage>(x => x.Method == method),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, _) => { sentRequests?.Add(message); })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }
    }
}