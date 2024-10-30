public interface ISortable<T>
{
    public bool IsLower(T otherObject);
    public bool IsHigher(T otherObject);
}
