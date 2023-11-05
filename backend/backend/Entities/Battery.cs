namespace backend.Entities
{
    [BsonCollection("battery")]
    public class Battery : Document
    {
        public int Value { get; set; }
    }
}

