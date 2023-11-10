using backend.Entities;
using MongoDB.Bson;


namespace Backend.Models
{
    [Serializable]
    public class PresentationModel
    {
        //public ObjectId Id { get; set; }

        public DateTime Time { get; set; }

        public decimal Value { get; set; }

        public int Instance {  get; set; }
        public string Type { get; set; }

        public PresentationModel(Measurement measurement) {
            // this.Id = measurement.Id;
            this.Type = measurement.SensorType;
            this.Instance = measurement.Instance;
            this.Time = measurement.Time;
            this.Value = measurement.Value;
        }
        
    }
}
