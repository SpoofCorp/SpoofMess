using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.Changeable;

public abstract class ChangeableEntity : IChangeable
{
    public DateTime LastModified { get; set; }
}
