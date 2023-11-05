using backend.Entities;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

namespace backend.Services
{
    public class SensorReaderBackgroundService : BackgroundService
    {
        private readonly IConfiguration Configuration;

        public SensorReaderBackgroundService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
        {


            MqttApplicationMessage message = e.ApplicationMessage;
            QueueMessage queueMessage = JsonSerializer.Deserialize<QueueMessage>(message.Payload);

            Console.WriteLine(message.Topic + ":" + queueMessage.Message);
            //switch (message.Topic)
            //{
            //    //case "":

            //    //case "2":

            //    //default:
            //}

            return;
        }
        


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var configSection = Configuration.GetSection("MessageQueue");
            string mqAddress = configSection["Address"];
            int mqPort = Int32.Parse(configSection["Port"]);


            var mqttFactory = new MqttFactory();

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                //try{} catch (Exception e) { Console.WriteLine("couldnt connect retrying in 5")}

                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(mqAddress,mqPort).Build();

                mqttClient.ApplicationMessageReceivedAsync += HandleMessage;  

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter( f => { f.WithTopic("temperature"); })
                    .WithTopicFilter( f => { f.WithTopic("altitude"); })
                    .WithTopicFilter( f => { f.WithTopic("distance"); })
                    .WithTopicFilter( f => { f.WithTopic("battery");})
                    .Build();

                await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                //Wait For Cancellation
                while (!stoppingToken.IsCancellationRequested) { 
                    
                }

                await mqttClient.DisconnectAsync();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }

}
