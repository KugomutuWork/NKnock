var builder = WebApplication.CreateBuilder(args);

// Swagger (OpenAPI + UI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// いったんHTTPSリダイレクトは切る（今回の詰まり回避）
/* app.UseHttpsRedirection(); */

app.MapGet("/health", () => Results.Ok("ok"))
   .WithName("Health");

app.Run();
