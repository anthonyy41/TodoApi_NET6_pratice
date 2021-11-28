// import other package
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// GET with default route
app.MapGet("/", () => "load test");

app.MapGet("/alltodoitem", async (TodoDb db) =>
await db.Todos.ToListAsync());

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// 若出現錯誤時需要另外安裝套件 指令 : Install-Package Swashbuckle.AspNetCore -Version 6.2.3(版本)
builder.Services.AddSwaggerGen();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// example code
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
});

app.Run();

// create todo db class
class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsCompleted { get; set; }
    // public int Total { get; set; }
}

// create dbcontext with todo class
class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options) : base(options)
    {

    }

    public DbSet<Todo> Todos => Set<Todo>();
}

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}