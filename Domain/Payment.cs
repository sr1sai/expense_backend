using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PaymentDTO
    {
        public Guid UserId { get; set; }
        public Guid MessageId { get; set; }
    }
    public class Payment : PaymentDTO
    {
        public Guid Id { get; set; }
    }
}
