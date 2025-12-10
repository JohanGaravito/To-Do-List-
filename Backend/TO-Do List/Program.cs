using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TO_Do_List.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var cs = builder.Configuration.GetConnectionString("connectionDB")
      ?? builder.Configuration["ConnectionStrings:connectionDB"];

if (string.IsNullOrWhiteSpace(cs))
{
    Console.WriteLine(builder.Configuration.GetDebugView());
    throw new InvalidOperationException("Falta connectionDB en appsettings.*");
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 👇 Configura explícitamente el doc v1
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API ToDo",
        Version = "v1"
    });
});

builder.Services.AddDbContext<DBContext>(o => o.UseSqlServer(cs));

var app = builder.Build();

using (var s = app.Services.CreateScope())
{
    var db = s.ServiceProvider.GetRequiredService<DBContext>();
    Console.WriteLine($"DB={db.Database.GetDbConnection().Database} on {db.Database.GetDbConnection().DataSource}");
    db.Database.EnsureCreated();
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API ToDo v1");
    c.RoutePrefix = "swagger";   // 👈 ahora será /swagger
});

// 👇 Usa el nombre correcto de la política
app.UseCors("NuevaPolitica");

app.MapControllers();

app.Run("https://localhost:5051");
