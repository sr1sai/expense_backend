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
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("GetTransactions")]
        public Response<List<Transaction>> GetTransactions(Guid userId)
        {
            _logger.LogInformation("GetTransactions requested for UserId: {UserId} at {Timestamp}", userId, DateTime.UtcNow);
            try
            {
                var result = _transactionService.GetTransactions(userId);
                _logger.LogInformation("GetTransactions completed for UserId: {UserId}, Status: {Status}, Count: {Count}", 
                    userId, result.status, result.data?.Count ?? 0);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetTransactions for UserId: {UserId}", userId);
                throw;
            }
        }

        [HttpPost]
        [ActionName("AddTransaction")]
        public Response<Guid> AddTransaction([FromBody] TransactionDTO transaction)
        {
            _logger.LogInformation("AddTransaction requested for UserId: {UserId}, Amount: {Amount}, Type: {Type}, Target: {Target} at {Timestamp}", 
                transaction.UserId, transaction.Amount, transaction.Type, transaction.Target, DateTime.UtcNow);
            try
            {
                var result = _transactionService.AddTransaction(transaction);
                _logger.LogInformation("AddTransaction completed - Status: {Status}, TransactionId: {TransactionId}", 
                    result.status, result.data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AddTransaction for UserId: {UserId}", transaction.UserId);
                throw;
            }
        }

    }
}
