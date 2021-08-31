using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Utils
{
    
    internal static class ObjectExtensions
    {
        private static string ToJsonString(this object obj) =>
            JsonConvert.SerializeObject(obj, Formatting.None);

        private static string ToXmlString<T>(this T obj, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var serializer = new XmlSerializer(typeof(T));
            using var stringWriter = new StringWriterWithEncoding(encoding);
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        internal static string Serialize<T>(this T obj, Serialization serialization) =>
            serialization switch
            {
                Serialization.Json => obj.ToJsonString(),
                Serialization.Xml => obj.ToXmlString(),
                _ => throw new ArgumentOutOfRangeException(nameof(serialization), serialization, null)
            };
    }
    
    public sealed class StringWriterWithEncoding : StringWriter
    {
        public override Encoding Encoding { get; }

        public StringWriterWithEncoding (Encoding encoding)
        {
            Encoding = encoding;
        }    
    }
}