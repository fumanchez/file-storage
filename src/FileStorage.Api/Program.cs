using FileStorage.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<IStorage, SqliteStorage>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();