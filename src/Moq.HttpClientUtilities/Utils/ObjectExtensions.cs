using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Moq.HttpClientUtilities.Json;
using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Utils
{
    
    internal static class ObjectExtensions
    {
        public static string ToJsonString(this object obj) =>
            JsonConvert.SerializeObject(obj, Formatting.None, SupportedConverters);

        public static string ToXmlString<T>(this T obj, Encoding encoding = null)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriterWithEncoding(encoding);
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        private static readonly JsonConverter[] SupportedConverters = {
            new DateOnlyJsonConverter(),
            new NullableDateOnlyJsonConverter(),
            new TimeOnlyJsonConverter(),
            new NullableTimeOnlyJsonConverter()
        };
    }
    
    public sealed class StringWriterWithEncoding : StringWriter
    {
        public override Encoding Encoding { get; }

        public StringWriterWithEncoding (Encoding encoding = null)
        {
            Encoding = encoding ?? Encoding.UTF8;
        }    
    }
}