namespace RubeesEat.Model.EqualityComparer;

public class IdEqualityComparer<T> : IEqualityComparer<T> where T : class
{
    private readonly Func<T, object> _getId;

    public IdEqualityComparer(Func<T, object> getId)
    {
        _getId = getId;
    }

    public bool Equals(T x, T y)
    {
        return Equals(_getId(x), _getId(y));
    }

    public int GetHashCode(T obj)
    {
        return _getId(obj)?.GetHashCode() ?? 0;
    }
}
