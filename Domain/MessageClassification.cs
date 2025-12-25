using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MessageClassification
    {
        public string Payment { get; } = "Payment";
        public string Non_Payment { get; } = "Non-Payment";
        public string Unknown { get; } = "Unknown";
    }
}
