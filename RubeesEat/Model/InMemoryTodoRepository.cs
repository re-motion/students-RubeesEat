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

    public TodoItem? GetById(Guid guid)
    {
        return _todoItems.GetValueOrDefault(guid);
    }

    public bool Delete(Guid id)
    {
        return _todoItems.Remove(id);
    }

    public void Update(TodoItem todo)
    {
        if (!_todoItems.ContainsKey(todo.Id))
        {
            throw new ArgumentException("Not a valid guid.");
        }

        _todoItems[todo.Id] = todo;
    }
}
