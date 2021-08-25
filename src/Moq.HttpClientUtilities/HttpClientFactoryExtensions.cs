using System;
using System.Net.Http;

namespace Moq.HttpClientUtilities
{
    public static class HttpClientFactoryExtensions
    {
        public static HttpClient CreateHttpClient(this Mock<HttpMessageHandler> handler, string baseUri) =>
            CreateHttpClient(handler, new Uri(baseUri));

        public static HttpClient CreateHttpClient(this Mock<HttpMessageHandler> handler, Uri baseUri) => 
            new HttpClient(handler.Object) { BaseAddress = baseUri };

        public static IHttpClientFactory CreateHttpClientFactory(this Mock<HttpMessageHandler> handler, string baseUri) =>
            CreateHttpClientFactory(handler, new Uri(baseUri));

        public static IHttpClientFactory CreateHttpClientFactory(this Mock<HttpMessageHandler> handler, Uri baseUri)
        {
            var httpClient = handler.CreateHttpClient(baseUri);
            
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return mockHttpClientFactory.Object;
        }
    }
}