using backend.Entities;
using backend.Repositories;
using Backend.Configuration;
using MongoDB.Bson.Serialization.Serializers;
using MQTTnet;
using MQTTnet.Client;
using System.Globalization;
using System.Text.Json;

namespace backend.Services
{
    public class SensorReaderBackgroundService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        DatabaseRepository<Measurement> _repository;

        public SensorReaderBackgroundService(IConfiguration configuration,
            DatabaseRepository<Measurement> repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            MqttApplicationMessage message = e.ApplicationMessage;
            QueueMessage queueMessage = JsonSerializer.Deserialize<QueueMessage>(message.Payload);

            var val = double.Parse(queueMessage.Message, CultureInfo.InvariantCulture);
            _repository.Add(new Measurement() { Value = val, Time = queueMessage.Time, Instance = queueMessage.Instance, SensorType = message.Topic });

            Console.WriteLine(message.Topic + ":" + queueMessage.Instance + ":" + queueMessage.Message);
            return;
        }



        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var mqConfig = _configuration.GetSection("MessageQueue").Get<MessageQueueConfig>();
            string mqAddress = mqConfig.Address;
            int mqPort = mqConfig.Port;


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
                            Console.WriteLine($"Couldn't connect to message queue retrying in {i}");
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
