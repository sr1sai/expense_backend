using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ITransactionService
    {
        Response<List<Transaction>> GetTransactions(Guid userId);
        Response<Guid> AddTransaction(TransactionDTO transaction);
    }
}
