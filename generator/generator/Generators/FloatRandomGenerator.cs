using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    internal class FloatRandomGenerator : Generator<float>
    {
        public FloatRandomGenerator(IMqttClient mqttClient, Random random) : base (mqttClient,random) {}
        protected override float Parse(string value) => float.Parse(value);
        public override async void Generate()
        {
            var value = random.NextDouble() * (TopValue - BottomValue) + BottomValue;
            await Publish(value.ToString());
        }
    }
}
