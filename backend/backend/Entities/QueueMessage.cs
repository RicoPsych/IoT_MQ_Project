using System.Runtime.Serialization;

namespace backend.Entities
{

    [Serializable]
    public class QueueMessage
    {
        public DateTime Time { get; set; }
        public int Instance { get; set; }
        public string Message { get; set; }
    }
}
