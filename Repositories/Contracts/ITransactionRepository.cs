using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ITransactionRepository
    {
        List<Transaction> GetTransactions(Guid userId);
        Guid AddTransaction(TransactionDTO transaction);
    }
}
