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

    var temperatureSensors = new List<FloatRandomGenerator>();
    var distanceSensors = new List<FloatRandomGainGenerator>();
    var batterySensors = new List<IntegerRandomGainGenerator>();
    var altitudeSensors = new List<IntegerRandomGenerator>();


    for (int i = 0;i<4; i++)
    {
        var temperature = new FloatRandomGenerator(mqttClient, random)
        {
            Topic = "temperature",
            Instance = i + 1
        };
        temperatureSensors.Add(temperature);
        temperature.SetParams(configuration.GetSection($"Sensors:Temperature:Instances:{i+1}"));
        
        var battery = new IntegerRandomGainGenerator(mqttClient, random)
        {
            Topic = "battery",
            Instance = i+1
        };

        batterySensors.Add(battery);
        battery.SetParams(configuration.GetSection($"Sensors:Battery:Instances:{i + 1}"));

        var altitude = new IntegerRandomGenerator(mqttClient, random)
        {
            Topic = "altitude",
            Instance = i+1
        };
        altitudeSensors.Add(altitude);
        altitude.SetParams(configuration.GetSection($"Sensors:Altitude:Instances:{i + 1}"));

        var distance = new FloatRandomGainGenerator(mqttClient, random)
        {
            Topic = "distance",
            Instance = i + 1
        };
        distanceSensors.Add(distance);
        distance.SetParams(configuration.GetSection($"Sensors:Distance:Instances:{i + 1}"));
    }



    temperatureSensors.ForEach(sensor => sensor.StartTimer());
    altitudeSensors.ForEach(sensor => sensor.StartTimer());
    distanceSensors.ForEach(sensor => sensor.StartTimer());
    batterySensors.ForEach(sensor => sensor.StartTimer());
    ////temperature.StartTimer();
    //battery.StartTimer();
    //altitude.StartTimer();
    //distance.StartTimer();  

    //List<Timer> timers = new List<Timer>() { temperature.timer, altitude.timer, battery.timer, distance.timer};


    string input = "";
    while (input != "q")
    {
        //Reload Config
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        temperatureSensors.ForEach(sensor => sensor.SetParams(configuration.GetSection($"Sensors:Temperature:Instances:{sensor.Instance}")));
        altitudeSensors.ForEach(sensor => sensor.SetParams(configuration.GetSection($"Sensors:Altitude:Instances:{sensor.Instance}")));
        distanceSensors.ForEach(sensor => sensor.SetParams(configuration.GetSection($"Sensors:Distance:Instances:{sensor.Instance}")));
        batterySensors.ForEach(sensor => sensor.SetParams(configuration.GetSection($"Sensors:Battery:Instances:{sensor.Instance}")));


        //        Console.WriteLine("Started, enter q to exit");
        //        input = Console.ReadLine(); 
        Thread.Sleep(1000);
    }
    //    timers.ForEach(timer => timer.Change(-1, -1));
    //temperature.StopTimer();
    //battery.StopTimer();
    //altitude.StopTimer();
    //distance.StopTimer();

    temperatureSensors.ForEach(sensor => sensor.StopTimer());
    altitudeSensors.ForEach(sensor => sensor.StopTimer());
    distanceSensors.ForEach(sensor => sensor.StopTimer());
    batterySensors.ForEach(sensor => sensor.StopTimer());


    await mqttClient.DisconnectAsync();
}
