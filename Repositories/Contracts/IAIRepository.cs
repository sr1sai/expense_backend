using Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IAIRepository
    {
        Task<string> ClassifyMessage(MessageDTO message);

        Task<Transaction> ExtractTransaction(Message message);
    }
}
