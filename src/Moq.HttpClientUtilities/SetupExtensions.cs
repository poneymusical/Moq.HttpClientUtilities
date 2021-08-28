using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq.HttpClientUtilities.Utils;
using Moq.Protected;

namespace Moq.HttpClientUtilities
{
    public static class SetupExtensions
    {
        internal const string SendAsync = "SendAsync";


        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Uses the GET method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string path, HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, path, statusCode, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Uses the GET method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string path, HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, path, statusCode, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Uses the GET method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string path, HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, path, statusCode, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Uses the GET method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string path, HttpStatusCode statusCode, HttpContent content = null,
            IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, path, statusCode, content, sentRequests);


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
                .Callback<HttpRequestMessage, CancellationToken>((message, token) => { sentRequests?.Add(message); })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }


        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, new ByteArrayContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, new StringContent(content), sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any HTTP method and any path.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpContent content = null, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, content, sentRequests);


        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any HTTP method and any path.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
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
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, token) => { sentRequests?.Add(message); })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }


        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any GET request.
        /// </summary>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any GET request.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any GET request.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a OK response back on any GET request.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpContent content = null, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(handler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);


        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized to JSON.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(handler, HttpMethod.Get, statusCode, content);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(handler, HttpMethod.Get, statusCode, content);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(handler, HttpMethod.Get, statusCode, content);

        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back on any GET request.
        /// </summary>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, HttpContent content = null, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(handler, HttpMethod.Get, statusCode, content);

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
            SetupResponseOnAnyURI(handler, method, statusCode, new ByteArrayContent(content),
                sentRequests);

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
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(x => x.Method == method),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, token) => { sentRequests?.Add(message); })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }
    }
}