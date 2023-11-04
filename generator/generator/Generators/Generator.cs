using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    public abstract class Generator<T>
    {
        protected IMqttClient mqttClient;

        protected Random random;

        public T TopValue { get; set; }
        public T BottomValue { get; set; }
        public float Frequency { get; set; }

        public string Topic { get; set; }

        public TimeSpan PerMinute { get { return TimeSpan.FromSeconds(60 / Frequency); } }

        public abstract void SetParams(IConfigurationSection config);
        

        public abstract void Generate();

        protected async Task Publish(string value)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(value)
                .Build();

            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            Console.WriteLine($"Published: {Topic} = {value}");
            return;
        }

    }
}
