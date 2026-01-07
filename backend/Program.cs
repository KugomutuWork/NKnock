using backend.Endpoints;
using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger (OpenAPI + UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TodoStore>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // いったんオフでOK

app.MapGet("/health", () => Results.Ok("ok"))
   .WithName("Health");

// ここでTodoのルーティングをまとめて登録
app.MapTodoEndpoints();

app.Run();
