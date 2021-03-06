using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Moq.HttpClientUtilities.Json;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    internal const string DateFormat = "yyyy-MM-dd";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer) =>
        DateOnly.ParseExact((string) reader.Value!, DateFormat, CultureInfo.InvariantCulture);

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer) =>
        writer.WriteValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
}