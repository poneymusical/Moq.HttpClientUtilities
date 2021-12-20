using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Json;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    internal const string TimeFormat = "HH:mm:ss.FFF";

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer) =>
        TimeOnly.ParseExact((string) reader.Value!, TimeFormat, CultureInfo.InvariantCulture);

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer) =>
        writer.WriteValue(value.ToString(TimeFormat, CultureInfo.InvariantCulture));
}