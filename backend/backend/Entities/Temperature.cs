using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Entities
{
    [BsonCollection("temperatures")]
    public class Temperature : Document
    {
        //[BsonRepresentation(BsonType.Decimal128)]
        public decimal Value { get; set; }
        
    }
}
