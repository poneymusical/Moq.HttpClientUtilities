using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq.HttpClientUtilities.Utils;

namespace Moq.HttpClientUtilities.Tests
{
    internal class TestUtils
    {
        public static void CheckResult(List<HttpRequestMessage> sentRequests, HttpRequestMessage request,
            HttpResponseMessage response, HttpStatusCode statusCode, HttpContent content)
        {
            CheckResult_Base(sentRequests, request, response, statusCode);
            response.Content.Should().BeEquivalentTo(content);
        }

        public static async Task CheckResult(List<HttpRequestMessage> sentRequests, HttpRequestMessage request,
            HttpResponseMessage response, HttpStatusCode statusCode, string content)
        {
            CheckResult_Base(sentRequests, request, response, statusCode);
            (await response.Content.ReadAsStringAsync()).Should().BeEquivalentTo(content);
        }

        public static async Task CheckResult(List<HttpRequestMessage> sentRequests, HttpRequestMessage request,
            HttpResponseMessage response, HttpStatusCode statusCode, byte[] content)
        {
            CheckResult_Base(sentRequests, request, response, statusCode);
            (await response.Content.ReadAsByteArrayAsync()).Should().BeEquivalentTo(content);
        }

        public static async Task CheckResult(List<HttpRequestMessage> sentRequests, HttpRequestMessage request,
            HttpResponseMessage response, HttpStatusCode statusCode, object content) =>
            await CheckResult(sentRequests, request, response, statusCode, content.ToJsonString());

        private static void CheckResult_Base(List<HttpRequestMessage> sentRequests, HttpRequestMessage request,
            HttpResponseMessage response, HttpStatusCode statusCode)
        {
            sentRequests.Should().HaveCount(1);
            sentRequests.Should().Contain(request);
            response.StatusCode.Should().Be(statusCode);
        }

        public static IEnumerable<object[]> StatusCodes()
        {
            foreach (var statusCode in Enum.GetValues<HttpStatusCode>())
                yield return new object[] { statusCode };
        }

        public static IEnumerable<object[]> StatusCodesAndMethods()
        {
            foreach (var statusCode in Enum.GetValues<HttpStatusCode>())
                foreach (var method in HttpMethods)
                    yield return new object[] { statusCode, method };
        }

        public static IEnumerable<object[]> Methods()
        {
            foreach (var method in HttpMethods)
                yield return new object[] {method};
        }

        public static HttpMethod DifferentVerb(HttpMethod method)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            var index = new Random(DateTime.Today.Millisecond).Next(0, HttpMethods.Length - 1);
            return HttpMethods.Where(m => !m.Equals(method))
                .ElementAt(index);
        }

        private static readonly HttpMethod[] HttpMethods = {
            HttpMethod.Get,
            HttpMethod.Delete,
            HttpMethod.Head,
            HttpMethod.Options,
            HttpMethod.Patch,
            HttpMethod.Post,
            HttpMethod.Put,
            HttpMethod.Trace,
        };
    }
}