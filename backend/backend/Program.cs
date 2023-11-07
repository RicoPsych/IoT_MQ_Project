using backend.Entities;
using backend.Repositories;
using backend.Services;
using Backend.Configuration;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var builder = WebApplication.CreateBuilder();

var webHostConfiguration = configuration.GetSection("Backend").Get<WebHostConfig>();

builder.WebHost.UseUrls(webHostConfiguration.Urls);

builder.Services.Configure<IConfiguration>(configuration);
builder.Services.AddSingleton<IDatabaseRepository<Temperature>, DatabaseRepository<Temperature>>();
builder.Services.AddSingleton<IDatabaseRepository<Altitude>, DatabaseRepository<Altitude>>();
builder.Services.AddSingleton<IDatabaseRepository<Battery>, DatabaseRepository<Battery>>();
builder.Services.AddSingleton<IDatabaseRepository<Distance>, DatabaseRepository<Distance>>();

builder.Services.AddHostedService<SensorReaderBackgroundService>();


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
