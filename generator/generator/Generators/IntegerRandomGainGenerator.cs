using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    public class IntegerRandomGainGenerator : Generator<int>
    {
        public IntegerRandomGainGenerator(IMqttClient mqttClient, Random random)
        {
            this.mqttClient = mqttClient;
            this.random = random;
        }

        public int StartValue { get; set; }

        public override async void Generate()
        {
            StartValue += random.Next(BottomValue, TopValue + 1);
            await Publish(StartValue.ToString());
        }
    }
}
