using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace backend.Entities
{
    [BsonCollection("distances")]
    public class Distance : Document
    {
        //[BsonRepresentation(BsonType.Decimal128)]
        public decimal Value { get; set; }
    }
}

