using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.HttpClientUtilities.Utils;
using Moq.Protected;

namespace Moq.HttpClientUtilities
{
    public static class SetupResponseExtensions
    {
        internal const string SendAsync = "SendAsync";
        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Matches the method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponse(this Mock<HttpMessageHandler> handler,
            HttpMethod method, string path, HttpStatusCode statusCode, object content, 
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, method, path, statusCode, content.ToJsonString(), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Matches the method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponse(this Mock<HttpMessageHandler> handler,
            HttpMethod method, string path, HttpStatusCode statusCode, byte[] content,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, method, path, statusCode, new ByteArrayContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Matches the method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponse(this Mock<HttpMessageHandler> handler,
            HttpMethod method, string path, HttpStatusCode statusCode, string content,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, method, path, statusCode, new StringContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Matches the method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponse(this Mock<HttpMessageHandler> handler,
            HttpMethod method, string path, HttpStatusCode statusCode, HttpContent content = null,
            IList<HttpRequestMessage> sentRequests = null)
        {
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync,
                    ItExpr.Is<HttpRequestMessage>(x => x.Method == method && x.MatchesUri(path)),
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