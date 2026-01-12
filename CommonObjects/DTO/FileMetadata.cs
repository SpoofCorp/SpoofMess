namespace CommonObjects.DTO;

public class FileMetadata
{
    public Guid Id { get; set; }    

    public required long Size { get; init; }
}
