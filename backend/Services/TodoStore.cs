using backend.Models;

namespace backend.Services;

public class TodoStore
{
    private readonly List<Todo> _todos = new()
    {
        new(1, "First task", false),
        new(2, "Second task", true),
    };

    public IReadOnlyList<Todo> GetAll() => _todos;

    public Todo? GetById(int id) => _todos.FirstOrDefault(t => t.Id == id);

    public Todo Add(string title)
    {
        var nextId = _todos.Count == 0 ? 1 : _todos.Max(t => t.Id) + 1;
        var todo = new Todo(nextId, title.Trim(), false);
        _todos.Add(todo);
        return todo;
    }

    public Todo? SetDone(int id, bool done)
    {
        var index = _todos.FindIndex(t => t.Id == id);
        if (index < 0) return null;

        var updated = _todos[index] with { Done = done };
        _todos[index] = updated;
        return updated;
    }

    public bool Delete(int id)
    {
        var index = _todos.FindIndex(t => t.Id == id);
        if (index < 0) return false;

        _todos.RemoveAt(index);
        return true;
    }
}
