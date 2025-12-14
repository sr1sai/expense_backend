using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TransactionDTO
    {
        public Guid UserId { get; set; }
        public Guid MessageId { get; set; }
        public string Type{ get; set; }
        public double Amount{ get; set; }
        public string Account { get; set; }
        public string Target { get; set; }
        public DateTime Time { get; set; }
    }
    public class Transaction : TransactionDTO
    {
        public Guid Id { get; set; }
    }
}
