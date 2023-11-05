using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

namespace Generator.Generators
{
    public abstract class Generator<T>
    {
        protected IMqttClient mqttClient;

        protected Random random;

        public T TopValue { get; set; }
        public T BottomValue { get; set; }
        public float Frequency { get; set; }

        public int Instance {  get; set; }

        public string Topic { get; set; }

        public TimeSpan PerMinute { get { return TimeSpan.FromSeconds(60 / Frequency); } }

        public abstract void SetParams(IConfigurationSection config);
        

        public abstract void Generate();

        protected async Task Publish(string value)
        {
            QueueMessage queueMessage = new QueueMessage() { Instance = this.Instance , Message = value, Time = DateTime.Now};    


            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(JsonSerializer.SerializeToUtf8Bytes(queueMessage))
                .Build();

            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            Console.WriteLine($"Published: {Topic} = {value}");
            return;
        }

    }
}
