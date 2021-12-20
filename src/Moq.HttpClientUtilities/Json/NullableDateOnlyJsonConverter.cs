using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Json;

public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    internal const string DateFormat = "yyyy-MM-dd";
    
    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
            return null;
        return DateOnly.ParseExact((string)reader.Value, DateFormat, CultureInfo.InvariantCulture);
    }
    
    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer)
    {
        if (value != null)
            writer.WriteValue(value.Value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}