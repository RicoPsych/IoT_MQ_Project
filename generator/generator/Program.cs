// See https://aka.ms/new-console-template for more information
using MQTTnet.Client;
using MQTTnet;
using System.Net;
using System.Text;
using Generator;
using Microsoft.Extensions.Configuration;
using Generator.Generators;

Console.WriteLine("Hello, World!");

var mqttFactory = new MqttFactory();



if (Console.ReadLine() == "1")
{

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
        };
        var battery = new IntegerRandomGainGenerator(mqttClient, random)
        {
            Topic = "battery",
        };
        var altitude = new IntegerRandomGenerator(mqttClient, random)
        {
            Topic = "altitude",
        };
        var distance = new FloatRandomGainGenerator(mqttClient, random)
        {
            Topic = "distance"
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

            input = Console.ReadLine(); 

            timers.ForEach(timer => timer.Change(-1, -1));
        }

       




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