using System;
using System.Net.Http;

namespace Moq.HttpClientUtilities
{
    public static class HttpClientFactoryExtensions
    {
        private const string DefaultUri = "http://unittest";
        
        /// <summary>
        /// Creates a HttpClient that uses the mocked HttpMessageHandler.
        /// </summary>
        /// <param name="baseUri">The base URI passed to the HttpClient. This can be any valid URI; it will not be used.</param>
        /// <returns>A new instance of HttpClient that uses the mocked HttpMessageHandler.</returns>
        public static HttpClient CreateHttpClient(this Mock<HttpMessageHandler> handler, string baseUri = DefaultUri) =>
            CreateHttpClient(handler, new Uri(baseUri));

        private static HttpClient CreateHttpClient(this Mock<HttpMessageHandler> handler, Uri baseUri) => 
            new HttpClient(handler.Object) { BaseAddress = baseUri };

        /// <summary>
        /// Creates a IHttpClientFactory that always returns a HttpClient that uses the mocked HttpMessageHandler.
        /// </summary>
        /// <param name="baseUri">The base URI passed to the HttpClient. This can be any valid URI; it will not be used.</param>
        /// <returns>A new instance of HttpClient that uses the mocked HttpMessageHandler.</returns>
        public static IHttpClientFactory CreateHttpClientFactory(this Mock<HttpMessageHandler> handler, string baseUri = DefaultUri) =>
            CreateHttpClientFactory(handler, new Uri(baseUri));

        private static IHttpClientFactory CreateHttpClientFactory(this Mock<HttpMessageHandler> handler, Uri baseUri)
        {
            var httpClient = handler.CreateHttpClient(baseUri);
            
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return mockHttpClientFactory.Object;
        }
    }
}