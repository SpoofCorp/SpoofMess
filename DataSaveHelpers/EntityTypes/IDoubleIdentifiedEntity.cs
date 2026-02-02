namespace DataSaveHelpers.EntityTypes;

public interface IDoubleIdentified<T1, T2>
{
    public T1 Key1 { get; set; }
    public T2 Key2 { get; set; }
}