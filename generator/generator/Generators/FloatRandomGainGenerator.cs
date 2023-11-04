using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    internal class FloatRandomGainGenerator : Generator<float>
    {

        public FloatRandomGainGenerator(IMqttClient mqttClient, Random random)
        {
            this.mqttClient = mqttClient;
            this.random = random;
        }

        public double StartValue { get; set; }

        public override void SetParams(IConfigurationSection config)
        {
            Frequency = float.Parse(config["Frequency"]);
            TopValue = float.Parse(config["TopValue"]);
            BottomValue = float.Parse(config["BottomValue"]);
            StartValue = float.Parse(config["StartValue"]);
        }


        public override async void Generate()
        {
            StartValue += random.NextDouble() * (TopValue - BottomValue) + BottomValue;
            await Publish(StartValue.ToString());
        }
    }
}
