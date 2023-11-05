namespace backend.Entities
{
    [BsonCollection("altitudes")]
    public class Altitude : Document
    {
        public int Value { get; set; }
    }
}

