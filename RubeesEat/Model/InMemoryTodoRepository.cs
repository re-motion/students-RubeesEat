namespace RubeesEat.Model;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly Dictionary<Guid, TodoItem> _todoItems = new();

    public void Add(TodoItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _todoItems.TryAdd(item.Id, item);
    }

    public IEnumerable<TodoItem> GetAll()
    {
        return _todoItems.Values.OrderBy(e => e.Name);
    }
}
