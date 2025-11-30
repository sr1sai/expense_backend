using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MessageDTO
    {
        public Guid UserId { get; set; }
        public string MessageContent { get; set; }
    }
    public class Message : MessageDTO
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
    }
}
