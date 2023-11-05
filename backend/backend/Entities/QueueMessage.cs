using System.Runtime.Serialization;

namespace backend.Entities
{
    [DataContract]
    [Serializable]
    public class QueueMessage
    {
        public DateTime Time { get; set; }
        public int Instance { get; set; }
        public string Message { get; set; }
    }
}
