namespace backend.Entities
{
    [BsonCollection("distances")]
    public class Distance : Document
    {
        public decimal Value { get; set; }
    }
}

