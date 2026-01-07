using backend.Models;
using backend.Models.Requests;

namespace backend.Endpoints;

public static class TodoEndpoints
{
    // Program.cs から app.MapTodoEndpoints(); で呼べるようにする
    public static void MapTodoEndpoints(this WebApplication app)
    {
        // いまはDBの代わり：メモリ上の仮データ
        var todos = new List<Todo>
        {
            new(1, "First task", false),
            new(2, "Second task", true),
        };

        app.MapGet("/api/todos", () => Results.Ok(todos));

        app.MapGet("/api/todos/{id:int}", (int id) =>
        {
            var todo = todos.FirstOrDefault(t => t.Id == id);
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        });

        app.MapPost("/api/todos", (TodoCreateRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.Title))
                return Results.BadRequest(new { message = "Title is required." });

            var nextId = todos.Count == 0 ? 1 : todos.Max(t => t.Id) + 1;
            var todo = new Todo(nextId, req.Title.Trim(), false);
            todos.Add(todo);
            return Results.Created($"/api/todos/{todo.Id}", todo);
        });

        app.MapPatch("/api/todos/{id:int}/done", (int id, TodoDoneRequest req) =>
        {
            var index = todos.FindIndex(t => t.Id == id);
            if (index < 0) return Results.NotFound();

            var current = todos[index];
            var updated = current with { Done = req.Done };
            todos[index] = updated;
            return Results.Ok(updated);
        });

        app.MapDelete("/api/todos/{id:int}", (int id) =>
        {
            var index = todos.FindIndex(t => t.Id == id);
            if (index < 0) return Results.NotFound();

            todos.RemoveAt(index);
            return Results.NoContent();
        });
    }
}
