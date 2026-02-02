namespace DataSaveHelpers.EntityTypes;

public interface IChangeable
{
    public DateTime LastModified { get; set; }
}