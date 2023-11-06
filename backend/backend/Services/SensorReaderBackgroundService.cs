using backend.Entities;
using backend.Repositories;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

namespace backend.Services
{
    public class SensorReaderBackgroundService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseRepository<Temperature> _temperatureRepository;
        private readonly IDatabaseRepository<Altitude> _altitudeRepository;
        private readonly IDatabaseRepository<Distance> _distanceRepository;
        private readonly IDatabaseRepository<Battery> _batteryRepository;

        public SensorReaderBackgroundService(IConfiguration configuration,
            IDatabaseRepository<Temperature> temperatureRepository, 
            IDatabaseRepository<Altitude> altitudeRepository, 
            IDatabaseRepository<Distance> distanceRepository, 
            IDatabaseRepository<Battery> batteryRepository)
        {
            _configuration = configuration;
            _temperatureRepository = temperatureRepository;
            _altitudeRepository = altitudeRepository;
            _distanceRepository = distanceRepository;
            _batteryRepository = batteryRepository;
        }

        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {


            MqttApplicationMessage message = e.ApplicationMessage;
            QueueMessage queueMessage = JsonSerializer.Deserialize<QueueMessage>(message.Payload);

            Console.WriteLine(message.Topic + ":" + queueMessage.Message);


            switch (message.Topic)
            {
                case "temperature":
                    _temperatureRepository.Add(new Temperature { Value = (decimal)(double.Parse(queueMessage.Message)) });
                    break;
                case "altitude":
                    _altitudeRepository.Add(new Altitude { Value = int.Parse(queueMessage.Message) });
                    break;
                case "distance":
                    _distanceRepository.Add(new  Distance{ Value = (decimal)double.Parse(queueMessage.Message) });
                    break;
                case "battery":
                    _batteryRepository.Add(new Battery { Value = int.Parse(queueMessage.Message) });
                    break;
                default:
                    break;

            }

            return;
        }
        


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configSection = _configuration.GetSection("MessageQueue");
            string mqAddress = configSection["Address"];
            int mqPort = Int32.Parse(configSection["Port"]);


            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(mqAddress, mqPort).Build();

                mqttClient.ApplicationMessageReceivedAsync += HandleMessage;

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => { f.WithTopic("temperature"); })
                .WithTopicFilter(f => { f.WithTopic("altitude"); })
                .WithTopicFilter(f => { f.WithTopic("distance"); })
                .WithTopicFilter(f => { f.WithTopic("battery"); })
                .Build();




                while (!stoppingToken.IsCancellationRequested)
                {
                    try 
                    {
                        await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                        await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                        //Wait For Cancellation
                        while (!stoppingToken.IsCancellationRequested && mqttClient.IsConnected)
                        { }

                        if (mqttClient.IsConnected)
                        {
                            await mqttClient.DisconnectAsync();
                        }
                    }
                    catch (Exception e) {
                        for (int i = 5; i > 0; i--) {
                            Console.WriteLine($"Couldnt connect to message queue retrying in {i}");
                            Thread.Sleep(1000);
                        }
                        Console.WriteLine("Retrying");
                    };
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }

}
