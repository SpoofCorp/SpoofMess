namespace AdditionalHelpers.Services;

public interface ISerializer
{
    public string Serialize<T>(T obj);

    public T Deserialize<T>(string text);

    public T Deserialize<T>(byte[] body);
}
