namespace DataHelpers;

public abstract class SoftDeletableEntity : ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
