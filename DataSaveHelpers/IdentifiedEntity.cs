namespace DataSaveHelpers;

public abstract class IdentifiedEntity<TKey> : IIdentifiedEntity
{
    public TKey Id { get; set; } = default!;

    public string GetId() => $"{Id}";
}
