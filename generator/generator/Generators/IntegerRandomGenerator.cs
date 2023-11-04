using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    internal class IntegerRandomGenerator : Generator<int>
    {
        public IntegerRandomGenerator(IMqttClient mqttClient, Random random)
        {
            this.mqttClient = mqttClient;
            this.random = random;
        }

        public override void SetParams(IConfigurationSection config)
        {
            Frequency = float.Parse(config["Frequency"]);
            TopValue = Int32.Parse(config["TopValue"]);
            BottomValue = Int32.Parse(config["BottomValue"]);
        }

        public override async void Generate()
        {
            var value = random.Next(BottomValue, TopValue + 1);
            await Publish(value.ToString());
        }
    }
}
