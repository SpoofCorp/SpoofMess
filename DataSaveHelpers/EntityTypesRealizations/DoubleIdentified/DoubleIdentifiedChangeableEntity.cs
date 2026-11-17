namespace DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

public class DoubleIdentifiedChangeableEntity<T1, T2> : DoubleIdentifiedEntity<T1, T2>
{
    public DateTime LastModified { get; set; }
}