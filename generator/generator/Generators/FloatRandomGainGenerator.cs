using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;

namespace Generator.Generators
{
    internal class FloatRandomGainGenerator : Generator<double>
    {

        public FloatRandomGainGenerator(IMqttClient mqttClient, Random random) : base(mqttClient, random) {}

        public double StartValue { get; set; }
        public double CurrentValue { get; set; }    
        protected override double Parse(string value) => double.Parse(value);
        
        public override void SetParams(IConfigurationSection config)
        {
            //StartValue = float.Parse(config["StartValue"]);
            var newStartValue = double.Parse(config["StartValue"]);
            if (StartValue != newStartValue)
            {
                StartValue = newStartValue;
                CurrentValue = StartValue;
            }
            base.SetParams(config);
        }


        public override async void Generate()
        {
            CurrentValue += random.NextDouble() * (TopValue - BottomValue) + BottomValue;
            await Publish(CurrentValue.ToString());
        }
    }
}
