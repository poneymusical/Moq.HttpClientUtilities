using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Json;

public class NullableTimeOnlyJsonConverter : JsonConverter<TimeOnly?>
{
    internal const string TimeFormat = "HH:mm:ss.FFF";

    public override TimeOnly? ReadJson(JsonReader reader, Type objectType, TimeOnly? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        if (reader.Value == null)
            return null;
        return TimeOnly.ParseExact((string)reader.Value, TimeFormat, CultureInfo.InvariantCulture);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly? value, JsonSerializer serializer)
    {
        if (value != null)
            writer.WriteValue(value.Value.ToString(TimeFormat, CultureInfo.InvariantCulture));
    }
}