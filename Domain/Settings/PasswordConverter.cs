using System;
using System.Text;
using Newtonsoft.Json;

namespace Paragoniarz.Domain.Settings;

internal class PasswordConverter : JsonConverter<string>
{
    public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
    {
        if (value != null)
        {
            var encodedString = Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
            writer.WriteValue(encodedString);
        }
        else
            writer.WriteNull();
    }

    public override string? ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        string? value = reader.Value as string;
        if (string.IsNullOrEmpty(value))
            return null;

        return Encoding.UTF8.GetString(Convert.FromBase64String(value));
    }
}
