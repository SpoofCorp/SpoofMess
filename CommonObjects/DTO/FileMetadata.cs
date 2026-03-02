namespace CommonObjects.DTO;

public class FileMetadata
{
    public byte[] Id { get; set; }    

    public required long Size { get; init; }
}
