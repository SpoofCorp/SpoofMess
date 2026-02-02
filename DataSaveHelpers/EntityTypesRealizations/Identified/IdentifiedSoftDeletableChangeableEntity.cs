using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.Identified;

public abstract class IdentifiedSoftDeletableChangeableEntity<TKey> : IdentifiedSoftDeletableEntity<TKey>, IChangeable
{
    public DateTime LastModified { get; set; }
}
