using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using System.Collections;
using System.Text.Json;

namespace Generator.Generators
{
    public abstract class Generator<T>
    {
        protected IMqttClient mqttClient;

        protected Random random;

        public Generator(IMqttClient mqttClient, Random random)
        {
            this.mqttClient = mqttClient;
            this.random = random;
        }

        public T TopValue { get; set; }
        public T BottomValue { get; set; }
        public float Frequency { get; set; }

        public int Instance {  get; set; }

        public string Topic { get; set; }

        public TimeSpan PerMinute { get { return TimeSpan.FromSeconds(60 / Frequency); } }

        public LinkedList<Timer> timers { get; set; }
        public Timer timer { get; set; }

        protected abstract T Parse(string value);

        public void StartTimer()
        {
            timer = new Timer(_ => Generate(), "", TimeSpan.FromSeconds(1), PerMinute);
        }
        public void StopTimer()
        {
            timer.Change(-1, -1);
        }


        public virtual void SetParams(IConfigurationSection config)
        {
            TopValue = Parse(config["TopValue"]);
            BottomValue = Parse(config["BottomValue"]);
            
            var newFrequency = float.Parse(config["Frequency"]);

            if (newFrequency != Frequency)
            {
                Frequency = newFrequency;
                timer?.Change(TimeSpan.FromSeconds(0), PerMinute);
            }

        }
        
        
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
