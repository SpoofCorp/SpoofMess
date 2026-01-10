namespace DataHelpers;

public abstract class IdentifiedSoftDeletableEntity<TKey> : IdentifiedEntity<TKey>, ISoftDeletable
{
    public bool IsDeleted { get; set; }
}