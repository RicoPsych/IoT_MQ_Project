using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    [DataContract]
    [Serializable]
    public class QueueMessage
    {
        public DateTime Time {  get; set; }
        public int Instance { get; set; }
        public string Message { get; set; } 
    }
}
