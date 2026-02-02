using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

public class DoubleIdentifiedSoftDeletable<T1, T2> : DoubleIdentifiedEntity<T1, T2>, ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
