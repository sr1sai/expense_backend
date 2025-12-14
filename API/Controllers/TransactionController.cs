using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TransactionController: ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [ActionName("GetTransactions")]
        public Response<List<Transaction>> GetTransactions(Guid userId)
        {
            return _transactionService.GetTransactions(userId);
        }

        [HttpPost]
        [ActionName("AddTransaction")]
        public Response<Guid> AddTransaction([FromBody] TransactionDTO transaction)
        {
            return _transactionService.AddTransaction(transaction);
        }

    }
}
