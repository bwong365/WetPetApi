using System.Text.Json;
using System.Text.Json.Serialization;

namespace WetPet.Api.Util;

public class JsonStringTrimmingConverter : JsonConverter<string?>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return value?.Trim();
    }

    public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.Trim());
    }
}