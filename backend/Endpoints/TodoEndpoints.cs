using backend.Models.Requests;
using backend.Services;

namespace backend.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/todos", (TodoStore store) => Results.Ok(store.GetAll()));

        app.MapGet("/api/todos/{id:int}", (int id, TodoStore store) =>
        {
            var todo = store.GetById(id);
            return todo is null ? Results.NotFound() : Results.Ok(todo);
        });

        app.MapPost("/api/todos", (TodoCreateRequest req, TodoStore store) =>
        {
            if (string.IsNullOrWhiteSpace(req.Title))
                return Results.BadRequest(new { message = "Title is required." });

            var todo = store.Add(req.Title.Trim());
            return Results.Created($"/api/todos/{todo.Id}", todo);
        });

        app.MapPatch("/api/todos/{id:int}/done", (int id, TodoDoneRequest req, TodoStore store) =>
        {
            var updated = store.SetDone(id, req.Done);
            return updated is null ? Results.NotFound() : Results.Ok(updated);
        });

        app.MapDelete("/api/todos/{id:int}", (int id, TodoStore store) =>
        {
            var ok = store.Delete(id);
            return ok ? Results.NoContent() : Results.NotFound();
        });
    }
}
