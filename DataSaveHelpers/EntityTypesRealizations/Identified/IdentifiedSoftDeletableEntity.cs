using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.Identified;

public abstract class IdentifiedSoftDeletableEntity<TKey> : IdentifiedEntity<TKey>, ISoftDeletable
{
    public bool IsDeleted { get; set; }
}