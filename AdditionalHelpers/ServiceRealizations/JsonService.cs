using AdditionalHelpers.Services;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace AdditionalHelpers.ServiceRealizations;

public class JsonService : ISerializer
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

    public T? Deserialize<T>(string text)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(text, Options);
        }
        catch
        {
            throw new InvalidDataException($"Invalid json for type: {typeof(T).Name} json: {text}");
        }
    }

    public T? Deserialize<T>(byte[] body)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(body, Options);
        }
        catch
        {
            throw new InvalidDataException($"Invalid json for type: {typeof(T).Name} body: {body}");
        }
    }
}
