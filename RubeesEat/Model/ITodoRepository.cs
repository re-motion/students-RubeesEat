namespace RubeesEat.Model;

public interface ITodoRepository
{
    void Add(TodoItem item);

    IEnumerable<TodoItem> GetAll();
}
