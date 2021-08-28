using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Moq.HttpClientUtilities.Utils;

namespace Moq.HttpClientUtilities
{
    public static class SetupResponseOnGETExtensions
    {
        /// <summary>
        /// Sets up a HTTP response. The handler will send a response back only if the request:<br/>
        /// - Uses the GET method,<br/>
        /// - Matches the path.<br/>
        /// </summary>
        /// <param name="path">The URI path (without the base URI).</param>
        /// <param name="statusCode">The response status code.</param>
        /// <param name="content">The response content. The object will be serialized according to the selected mode.</param>
        /// <param name="mode">The serialization mode.</param>
        /// <param name="sentRequests">The request collection where the requests will be stored.</param>
        public static void SetupResponseOnGET(this Mock<HttpMessageHandler> handler,
            string path, HttpStatusCode statusCode, object content,
            Serialization mode = Serialization.Json, IList<HttpRequestMessage> sentRequests = null) =>
            SetupResponseOnGET(handler, path, statusCode, content.Serialize(mode), sentRequests);

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
            SetupResponseOnGET(handler, path, statusCode, new ByteArrayContent(content), sentRequests);

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
            SetupResponseOnGET(handler, path, statusCode, new StringContent(content), sentRequests);

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
            handler.SetupResponse(HttpMethod.Get, path, statusCode, content, sentRequests);
    }
}