using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    internal class IntegerRandomGenerator : Generator<int>
    {
        public IntegerRandomGenerator(IMqttClient mqttClient, Random random) : base(mqttClient, random) {}
        protected override int Parse(string value) => int.Parse(value);
        public override async void Generate()
        {
            var value = random.Next(BottomValue, TopValue + 1);
            await Publish(value.ToString());
        }
    }
}
