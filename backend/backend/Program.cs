using backend.Entities;
using backend.Repositories;
using backend.Services;




var builder = WebApplication.CreateBuilder(args);


var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.Configure<IConfiguration>(configuration);

builder.Services.AddSingleton<IDatabaseRepository<Temperature>, TemperatureRepository>();
builder.Services.AddSingleton<IDatabaseRepository<Altitude>, DatabaseRepository<Altitude>>();
builder.Services.AddSingleton<IDatabaseRepository<Battery>, DatabaseRepository<Battery>>();
builder.Services.AddSingleton<IDatabaseRepository<Distance>, DatabaseRepository<Distance>>();

builder.Services.AddHostedService<SensorReaderBackgroundService>();



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
