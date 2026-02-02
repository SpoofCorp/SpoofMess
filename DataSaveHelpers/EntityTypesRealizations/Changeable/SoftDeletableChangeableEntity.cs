using DataSaveHelpers.EntityTypes;
using DataSaveHelpers.EntityTypesRealizations.SoftDeletable;

namespace DataSaveHelpers.EntityTypesRealizations.Changeable;

public abstract class SoftDeletableChangeableEntity : SoftDeletableEntity, IChangeable
{
    public DateTime LastModified { get; set; }
}