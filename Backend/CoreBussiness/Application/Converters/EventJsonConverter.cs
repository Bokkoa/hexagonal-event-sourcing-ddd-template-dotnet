using Domain.Abstractions.Events;
using Domain.Modules.Todos.Events;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Converters;
public class EventJsonConverter : JsonConverter<BaseEvent>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
    }

    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
        {
            throw new JsonException($"Failed to parse {nameof(JsonDocument)} !");
        }

        if (!doc.RootElement.TryGetProperty("Type", out var type))
        {
            throw new JsonException("Could not detect the type discriminator property!");
        }

        var typeDiscriminator = type.GetString();
        var json = doc.RootElement.GetRawText();

        return typeDiscriminator switch
        {
            nameof(TodoCreatedEvent) => JsonSerializer.Deserialize<TodoCreatedEvent>(json, options),
            _ => throw new JsonException($"{typeDiscriminator} is not supported yet!"),
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
