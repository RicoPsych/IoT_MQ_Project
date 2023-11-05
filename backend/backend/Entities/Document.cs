using MongoDB.Bson;

namespace backend.Entities
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime Time { get; set; }


    }
}
