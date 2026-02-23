using AdditionalHelpers.Services;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AdditionalHelpers.ServiceRealizations;

public class JsonSerializerService : ISerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
    public string Serialize<T>(T obj) =>
        JsonSerializer.Serialize(obj, Options);

    public T Deserialize<T>(string text)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(text, Options) ?? throw new ArgumentNullException($"Data has been null: {text}");
        }
        catch
        {
            throw new InvalidDataException($"Invalid json for type: {typeof(T).Name} json: {text}");
        }
    }

    public T Deserialize<T>(byte[] body)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(body, Options) ?? throw new ArgumentNullException($"Data has been null: {body}");
        }
        catch
        {
            throw new InvalidDataException($"Invalid json for type: {typeof(T).Name} body: {body}");
        }
    }
}
