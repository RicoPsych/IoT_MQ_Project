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

        public override async void Generate()
        {
            StartValue += random.NextDouble() * (TopValue - BottomValue) + BottomValue;
            await Publish(StartValue.ToString());
        }
    }
}
