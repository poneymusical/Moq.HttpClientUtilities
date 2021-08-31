using System.Text;
using System.Xml.Serialization;
using AutoFixture;
using FluentAssertions;
using Moq.HttpClientUtilities.Utils;
using Newtonsoft.Json;
using Xunit;

namespace Moq.HttpClientUtilities.Tests
{
    public class ObjectExtensionsTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void Serialize_Json()
        {
            var obj = new { prop = _fixture.Create<string>() };
            var result = obj.Serialize(Serialization.Json);
            result.Should().Be(JsonConvert.SerializeObject(obj, Formatting.None));
        }

        [Fact]
        public void Serialize_Xml()
        {
            var obj = new MockObject(_fixture.Create<string>());

            var serializer = new XmlSerializer(typeof(MockObject));
            using var writer = new StringWriterWithEncoding(Encoding.UTF8);
            serializer.Serialize(writer, obj);
            var expected = writer.ToString();
            
            var result = obj.Serialize(Serialization.Xml);

            result.Should().Be(expected);
        }

        public class MockObject
        {
            public MockObject() { }

            public MockObject(string property)
            {
                Property = property;
            }

            public string Property { get; set; }
        }
    }
}