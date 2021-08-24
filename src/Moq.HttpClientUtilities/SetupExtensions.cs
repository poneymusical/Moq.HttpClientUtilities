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

        public static void SetupResponse(this Mock<HttpMessageHandler> handler, 
            HttpMethod method, string uri, HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, method, uri, statusCode, content.ToJsonString(), sentRequests);

        public static void SetupResponse(this Mock<HttpMessageHandler> handler, 
            HttpMethod method, string uri, HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, method, uri, statusCode, new ByteArrayContent(content), sentRequests);

        public static void SetupResponse(this Mock<HttpMessageHandler> handler, 
            HttpMethod method, string uri, HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, method, uri, statusCode, new StringContent(content), sentRequests);

        public static void SetupResponse(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpMethod method, string uri, HttpStatusCode statusCode, HttpContent content = null, IList<HttpRequestMessage> sentRequests = null)
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(x => x.Method == method && x.MatchesUri(uri)), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, token) =>
                {
                    sentRequests?.Add(message);
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }

        public static void SetupUbiquitousResponse(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, statusCode, content.ToJsonString(), sentRequests);

        public static void SetupUbiquitousResponse(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, statusCode, new ByteArrayContent(content), sentRequests);

        public static void SetupUbiquitousResponse(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, statusCode, new StringContent(content), sentRequests);

        public static void SetupUbiquitousResponse(this Mock<HttpMessageHandler> handler, 
            HttpStatusCode statusCode, HttpContent content = null, IList<HttpRequestMessage> sentRequests = null)
        {
            handler.Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, token) =>
                {
                    sentRequests?.Add(message);
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = content
                });
        }

        public static void SetupUbiquitousOKResponse(this Mock<HttpMessageHandler> handler,
            object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, HttpStatusCode.OK, content, sentRequests);

        public static void SetupUbiquitousOKResponse(this Mock<HttpMessageHandler> handler,
            byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, HttpStatusCode.OK, new ByteArrayContent(content), sentRequests);

        public static void SetupUbiquitousOKResponse(this Mock<HttpMessageHandler> handler,
            string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, HttpStatusCode.OK, new StringContent(content), sentRequests);

        public static void SetupUbiquitousOKResponse(this Mock<HttpMessageHandler> handler,
            HttpContent content = null, IList<HttpRequestMessage> sentRequests = null) =>
            SetupUbiquitousResponse(handler, HttpStatusCode.OK, content, sentRequests);



        /*
         * protected void SetupGetResponse(object content) =>
            SetupGetResponse(HttpStatusCode.OK, content);

        protected void SetupGetResponse(byte[] content) =>
            SetupGetResponse(HttpStatusCode.OK, content);

        protected void SetupGetResponse(string content) =>
            SetupGetResponse(HttpStatusCode.OK, content);

        protected void SetupGetResponse(HttpStatusCode statusCode, object content) =>
            SetupResponse(HttpMethod.Get, statusCode, content);

        protected void SetupGetResponse(HttpStatusCode statusCode, byte[] content) =>
            SetupResponse(HttpMethod.Get, statusCode, content);

        protected void SetupGetResponse(HttpStatusCode statusCode, string content) =>
            SetupResponse(HttpMethod.Get, statusCode, content);

        protected void SetupResponse(HttpMethod method, HttpStatusCode statusCode, object content) =>
            SetupResponse(method, statusCode, JToken.FromObject(content).ToString(Formatting.None));

        protected void SetupResponse(HttpMethod method, HttpStatusCode statusCode, byte[] content) =>
            SetupResponse(method, statusCode, new ByteArrayContent(content));

        protected void SetupResponse(HttpMethod method, HttpStatusCode statusCode, string content) =>
            SetupResponse(method, statusCode, new StringContent(content));

        protected void SetupResponse(HttpMethod method, HttpStatusCode statusCode, HttpContent httpContent)
        {
            MockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(x => x.Method == method), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((message, token) => { SentMessage = message; })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = httpContent
                });
        }


        protected void SetupGetResponse(string uri, HttpStatusCode statusCode, object content) =>
            SetupResponse(HttpMethod.Get, uri, statusCode, content);

        protected void SetupGetResponse(string uri, HttpStatusCode statusCode, byte[] content) =>
            SetupResponse(HttpMethod.Get, uri, statusCode, content);

        protected void SetupGetResponse(string uri, HttpStatusCode statusCode, string content) =>
            SetupResponse(HttpMethod.Get, uri, statusCode, content);
         */
    }
}