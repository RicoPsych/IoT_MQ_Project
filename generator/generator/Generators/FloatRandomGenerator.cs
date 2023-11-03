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

        public override async void Generate()
        {
            var value = random.NextDouble() * (TopValue - BottomValue) + BottomValue;
            await Publish(value.ToString());
        }
    }
}
