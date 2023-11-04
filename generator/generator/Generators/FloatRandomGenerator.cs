using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    internal class FloatRandomGenerator : Generator<float>
    {

        public FloatRandomGenerator(IMqttClient mqttClient, Random random)
        {
            this.mqttClient = mqttClient;
            this.random = random;
        }

        public override void SetParams(IConfigurationSection config)
        {
            Frequency = float.Parse(config["Frequency"]);
            TopValue = float.Parse(config["TopValue"]);
            BottomValue = float.Parse(config["BottomValue"]);
        }


        public override async void Generate()
        {
            var value = random.NextDouble() * (TopValue - BottomValue) + BottomValue;
            await Publish(value.ToString());
        }
    }
}
