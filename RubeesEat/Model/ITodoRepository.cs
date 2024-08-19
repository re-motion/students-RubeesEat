namespace RubeesEat.Model;

public interface ITodoRepository
{
    void Add(TodoItem item);

    IEnumerable<TodoItem> GetAll();

    TodoItem? GetById(Guid guid);

    void Update(TodoItem todo);

    bool Delete(Guid id);
}
