namespace RubeesEat.Model;

public class TodoItem
{
    public static TodoItem Create(string name) => new(Guid.NewGuid(), name, false);

    public Guid Id { get; }

    public string Name { get; }

    public bool IsCompleted { get; }

    public TodoItem(Guid id, string name, bool isCompleted)
    {
        Id = id;
        Name = name;
        IsCompleted = isCompleted;
    }

    public TodoItem WithIsCompleted(bool isCompleted)
    {
        return new TodoItem(Id, Name, isCompleted);
    }
}
