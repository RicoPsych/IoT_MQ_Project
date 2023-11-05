// See https://aka.ms/new-console-template for more information
using MQTTnet.Client;
using MQTTnet;
using System.Net;
using System.Text;
using Generator;
using Microsoft.Extensions.Configuration;
using Generator.Generators;

Console.WriteLine("Starting");

var mqttFactory = new MqttFactory();


using (var mqttClient = mqttFactory.CreateMqttClient())
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var mqAddress = configuration.GetSection("Configuration")["Address"];
    var mqPort = Int32.Parse(configuration.GetSection("Configuration")["Port"]);

    var mqttClientOptions = new MqttClientOptionsBuilder()
        .WithTcpServer(mqAddress, mqPort)
        .Build();

    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

    var random = new Random();

    var temperature = new FloatRandomGenerator(mqttClient, random)    
    {
        Topic = "temperature",
        Instance = 1
    };
    var battery = new IntegerRandomGainGenerator(mqttClient, random)
    {
        Topic = "battery",
        Instance = 1
    };
    var altitude = new IntegerRandomGenerator(mqttClient, random)
    {
        Topic = "altitude",
        Instance = 1
    };
    var distance = new FloatRandomGainGenerator(mqttClient, random)
    {
        Topic = "distance",
        Instance = 1
    };


    string input = "";
    while (input != "q")
    {
        //Reload Config
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        temperature.SetParams(configuration.GetSection("Sensors").GetSection("Temperature"));
        battery.SetParams(configuration.GetSection("Sensors").GetSection("Battery"));
        altitude.SetParams(configuration.GetSection("Sensors").GetSection("Altitude"));
        distance.SetParams(configuration.GetSection("Sensors").GetSection("Distance"));


        List<Timer> timers = new List<Timer>() { new Timer(_ => temperature.Generate(), "", TimeSpan.FromSeconds(1), temperature.PerMinute),
                                                new Timer(_ => altitude.Generate(), "", TimeSpan.FromSeconds(1), altitude.PerMinute),
                                                new Timer(_ => battery.Generate(), "", TimeSpan.FromSeconds(1), battery.PerMinute),
                                                new Timer(_ => distance.Generate(), "", TimeSpan.FromSeconds(1), distance.PerMinute)
        };
        Console.WriteLine("Started, enter q to exit");
        input = Console.ReadLine(); 

        timers.ForEach(timer => timer.Change(-1, -1));
    }
    await mqttClient.DisconnectAsync();
}
