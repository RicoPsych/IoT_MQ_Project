using MongoDB.Bson;

namespace backend.Entities
{
    [BsonCollection("measurements")]
    public  class Measurement: IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime Time { get; set; }

        public int Instance {  get; set; }

        public string SensorType {  get; set; }

        public decimal Value { get; set; }
    }
}
