using Microsoft.EntityFrameworkCore;
using ExamenAPI.Database;
using ExamenAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Registrar el DbContext con SQLite
builder.Services.AddDbContext<SimulationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISimulationService, SimulationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();