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


        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string uri, HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, uri, statusCode, content, sentRequests);

        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string uri, HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, uri, statusCode, content, sentRequests);

        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string uri, HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, uri, statusCode, content, sentRequests);

        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string uri, HttpStatusCode statusCode, HttpContent content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponse(handler, HttpMethod.Get, uri, statusCode, content, sentRequests);



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



        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, content, sentRequests);

        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, new ByteArrayContent(content), sentRequests);

        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, new StringContent(content), sentRequests);

        public static void SetupOKResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpContent content = null, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, HttpStatusCode.OK, content, sentRequests);



        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, statusCode, content.ToJsonString(), sentRequests);

        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, statusCode, new ByteArrayContent(content), sentRequests);

        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler,
            HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyMethodAnyURI(handler, statusCode, new StringContent(content), sentRequests);

        public static void SetupResponseOnAnyMethodAnyURI(this Mock<HttpMessageHandler> handler, 
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



        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);

        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);

        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);

        public static void SetupOKResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpContent content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, HttpStatusCode.OK, content, sentRequests);



        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, statusCode, content);

        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, statusCode, content);

        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, statusCode, content);

        public static void SetupResponseOnGETAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpStatusCode statusCode, HttpContent content, IList<HttpRequestMessage> sentRequests = null)
            => SetupResponseOnAnyURI(mockHttpMessageHandler, HttpMethod.Get, statusCode, content);



        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpMethod method, HttpStatusCode statusCode, object content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, method, statusCode, content.ToJsonString(), sentRequests);

        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpMethod method, HttpStatusCode statusCode, byte[] content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, method, statusCode, new ByteArrayContent(content), sentRequests);

        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler,
            HttpMethod method, HttpStatusCode statusCode, string content, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnAnyURI(mockHttpMessageHandler, method, statusCode, new StringContent(content), sentRequests);

        public static void SetupResponseOnAnyURI(this Mock<HttpMessageHandler> mockHttpMessageHandler, 
            HttpMethod method, HttpStatusCode statusCode, HttpContent content, IList<HttpRequestMessage> sentRequests = null)
        {
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(SendAsync, ItExpr.Is<HttpRequestMessage>(x => x.Method == method), ItExpr.IsAny<CancellationToken>())
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
    }
}