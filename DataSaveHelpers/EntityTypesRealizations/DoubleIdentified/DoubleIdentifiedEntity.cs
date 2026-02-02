using DataSaveHelpers.EntityTypes;

namespace DataSaveHelpers.EntityTypesRealizations.DoubleIdentified;

public class DoubleIdentifiedEntity<T1, T2> : IDoubleIdentified<T1, T2>
{
    public T1 Key1 { get; set; } = default!;
    public T2 Key2 { get; set; } = default!;
}
