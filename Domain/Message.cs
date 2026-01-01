using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Domain
{
    public class MessageDTO
    {
        public Guid UserId { get; set; }
        public string Sender { get; set; } = string.Empty;
        public string MessageContent { get; set; } = string.Empty;
        public DateTime Time { get; set; }
    }
    public class Message : MessageDTO
    {
        public Guid Id { get; set; }
    }
}
