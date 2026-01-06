namespace CommonObjects.DTO;

public class FileMetadata
{
    public required string Name { get; init; }

    public required string Bucket { get; set; } = null!;

    public required string ObjectKey { get; set; } = null!;

    public required long Size { get; init; }
}
