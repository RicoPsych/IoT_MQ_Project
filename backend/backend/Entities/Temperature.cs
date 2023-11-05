using MongoDB.Bson;

namespace backend.Entities
{
    [BsonCollection("temperatures")]
    public class Temperature : Document
    {
        public decimal Value { get; set; }
    }
}
