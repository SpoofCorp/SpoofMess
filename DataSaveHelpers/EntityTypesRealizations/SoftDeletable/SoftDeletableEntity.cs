using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.SoftDeletable;

public abstract class SoftDeletableEntity : ISoftDeletable
{
    public bool IsDeleted { get; set; }
}