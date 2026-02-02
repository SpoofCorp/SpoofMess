using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

public class DoubleIdentifiedSoftDeletableChangeableEntity<T1, T2> : DoubleIdentifiedSoftDeletable<T1, T2>, ISoftDeletable
{
    public DateTime LastModified { get; set; }
}