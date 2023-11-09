using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    public class IntegerRandomGainGenerator : Generator<int>
    {
        public IntegerRandomGainGenerator(IMqttClient mqttClient, Random random) : base (mqttClient,random) {}
        public int StartValue { get; set; }
        public int CurrentValue { get; set; }   
        protected override int Parse(string value) => int.Parse(value);
        public override void SetParams(IConfigurationSection config)
        {
            var newStartValue = int.Parse(config["StartValue"]); 
            if (StartValue != newStartValue)
            {
                StartValue = newStartValue;
                CurrentValue = StartValue;
            }


            base.SetParams(config);
        }
        public override async void Generate()
        {
            CurrentValue += random.Next(BottomValue, TopValue + 1);
            await Publish(CurrentValue.ToString());
        }
    }
}
