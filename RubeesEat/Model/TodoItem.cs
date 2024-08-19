namespace RubeesEat.Model;

public class TodoItem(Guid id, string name, bool isCompleted)
{
    public static TodoItem Create(string name) => new(Guid.NewGuid(), name, false);

    public Guid Id { get; } = id;

    public string Name { get; } = name;

    public bool IsCompleted { get; } = isCompleted;

    public TodoItem WithIsCompleted(bool isCompleted)
    {
        return new TodoItem(Id, Name, isCompleted);
    }
}
