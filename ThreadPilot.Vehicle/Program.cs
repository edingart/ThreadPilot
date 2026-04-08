using Asp.Versioning;
using ThreadPilot.Vehicle.Repositories;
using ThreadPilot.Vehicle.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddScoped<IVehicleRepository, VehicleRepository>();

builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new(1, 0);
    opt.DefaultApiVersion = new(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
