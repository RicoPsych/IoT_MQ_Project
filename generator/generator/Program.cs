// See https://aka.ms/new-console-template for more information
using MQTTnet.Client;
using MQTTnet;
using System.Net;
using System.Text;
using Generator;
using Microsoft.Extensions.Configuration;
using Generator.Generators;

Console.WriteLine("Hello, World!");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var mqAddress = configuration.GetSection("Configuration")["Address"];
var mqPort = Int32.Parse(configuration.GetSection("Configuration")["Port"]);

var mqttFactory = new MqttFactory();

if (Console.ReadLine() == "1")
{

    using (var mqttClient = mqttFactory.CreateMqttClient())
    {

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(mqAddress, mqPort)
            .Build();

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var random = new Random();

        var temperature = new FloatRandomGenerator(mqttClient, random)    
        {
            Topic = "temperature",
            TopValue = 60,
            BottomValue = -20,

            Frequency = 60f
        };
        var battery = new IntegerRandomGainGenerator(mqttClient, random)
        {
            Topic = "battery",
            TopValue = 0,
            BottomValue = -2,

            StartValue = 100,
            
            Frequency = 1f
        };

        var altitude = new IntegerRandomGenerator(mqttClient, random)
        {
            Topic = "altitude",
            TopValue = 8848,
            BottomValue = -10994,

            Frequency = 30f
        };

        var distance = new FloatRandomGainGenerator(mqttClient, random)
        {
            Topic = "distance",
            TopValue = 1000f,
            BottomValue = 0f,
            StartValue = 900f,
            
            Frequency = 5f
        };

        List<Timer> timers = new List<Timer>() { new Timer(_ => temperature.Generate(), "", TimeSpan.FromSeconds(1), temperature.PerMinute),
                                                 new Timer(_ => altitude.Generate(), "", TimeSpan.FromSeconds(1), altitude.PerMinute),
                                                 new Timer(_ => battery.Generate(), "", TimeSpan.FromSeconds(1), battery.PerMinute),
                                                 new Timer(_ => distance.Generate(), "", TimeSpan.FromSeconds(1), distance.PerMinute)
        };

       

        Console.ReadLine();

        timers.ForEach(timer => timer.Change(-1, -1));
        await mqttClient.DisconnectAsync();

    }
}



else
{
    using (var mqttClient = mqttFactory.CreateMqttClient())
    {
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost", 1883).Build();

        // Setup message handling before connecting so that queued messages
        // are also handled properly. When there is no event handler attached all
        // received messages get lost.
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            Console.WriteLine(e.ApplicationMessage.ConvertPayloadToString());

            return Task.CompletedTask;
        };

        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

        var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(
                f =>
                {
                    f.WithTopic("battery");
                    f.WithTopic("temperature");
                    f.WithTopic("altitude");
                    f.WithTopic("distance");
                })
            .Build();

        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

        Console.WriteLine("MQTT client subscribed to topic.");

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }
}