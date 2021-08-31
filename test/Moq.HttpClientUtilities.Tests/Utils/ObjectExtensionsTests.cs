using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using AutoFixture;
using FluentAssertions;
using Moq.HttpClientUtilities.Utils;
using Newtonsoft.Json;
using Xunit;

namespace Moq.HttpClientUtilities.Tests.Utils
{
    public class ObjectExtensionsTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void ToJsonString()
        {
            var obj = new { prop = _fixture.Create<string>() };
            var result = obj.ToJsonString();
            result.Should().Be(JsonConvert.SerializeObject(obj, Formatting.None));
        }

        [Theory]
        [MemberData(nameof(Encodings))]
        public void ToXmlString(Encoding encoding)
        {
            var obj = _fixture.Create<MockObject>();

            string expected;
            var serializer = new XmlSerializer(typeof(MockObject));
            using (var writer = new StringWriterWithEncoding(encoding))
            {
                serializer.Serialize(writer, obj);
                expected = writer.ToString();
            }

            var result = obj.ToXmlString(encoding);
            result.Should().Be(expected);
        }

        public static IEnumerable<object[]> Encodings()
        {
            yield return new object[] { null };
            yield return new object[] { Encoding.UTF8 };
            yield return new object[] { Encoding.Default };
        }

        public class MockObject
        {
            public MockObject() { }

            public string Property { get; set; }
        }
    }
}