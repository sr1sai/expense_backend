using Domain;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TransactionService: ITransactionService 
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Response<Guid> AddTransaction(TransactionDTO transaction)
        {
            Response<Guid> response = new Response<Guid>();
            try
            {
                var result = _transactionRepository.AddTransaction(transaction);
                response.status = true;
                response.message = "Transaction added successfully.";
                response.data = result;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error adding transaction: {ex.Message}";
                response.data = Guid.Empty;
            }
            return response;
        }

        public Response<List<Transaction>> GetTransactions(Guid userId)
        {
            Response<List<Transaction>> response = new Response<List<Transaction>>();
            try
            {
                var result = _transactionRepository.GetTransactions(userId);
                response.status = true;
                response.message = "Transactions retrieved successfully.";
                response.data = result;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = $"Error retrieving transactions: {ex.Message}";
                response.data = new List<Transaction>();
            }
            return response;
        }
    }
}
