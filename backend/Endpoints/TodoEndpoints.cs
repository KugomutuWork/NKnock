using backend.Models.Requests;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/todos", async (AppDbContext db) =>
{
    var todos = await db.Todos.OrderByDescending(x => x.Id).ToListAsync();
    return Results.Ok(todos);
});

        app.MapGet("/api/todos/{id:int}", async (int id, AppDbContext db) =>
 {
     var todo = await db.Todos.FindAsync(id);
     return todo is null ? Results.NotFound() : Results.Ok(todo);
 });

        app.MapPost("/api/todos", async (TodoCreateRequest req, AppDbContext db) =>
    {
        if (string.IsNullOrWhiteSpace(req.Title))
            return Results.BadRequest(new { message = "Title is required." });

        var todo = new Todo { Title = req.Title.Trim(), Done = false };
        db.Todos.Add(todo);
        await db.SaveChangesAsync();

        return Results.Created($"/api/todos/{todo.Id}", todo);
    });

        app.MapPatch("/api/todos/{id:int}/done", async (int id, TodoDoneRequest req, AppDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();

            todo.Done = req.Done;
            await db.SaveChangesAsync();

            return Results.Ok(todo);
        });

        app.MapDelete("/api/todos/{id:int}", async (int id, AppDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();

            db.Todos.Remove(todo);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
