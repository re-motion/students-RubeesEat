using System.Collections;
using System.Collections.Immutable;
namespace RubeesEat.Model;

public class PaginatedView<T> : IEnumerable<T>
{
    public ImmutableArray<T> Items { get; }
    public int Page { get; }
    public int TotalPages { get; }

    public PaginatedView(ImmutableArray<T> items, int page, int totalPages)
    {
        Page = page;
        TotalPages = totalPages;
        Items = items;
    }

    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
    public int Count => Items.Length;
    public T this[int index]
    {
        get => Items[index];
    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)Items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
