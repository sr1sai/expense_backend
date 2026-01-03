using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController: ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [HttpPost]
        [ActionName("AddToPayments")]
        public Response<Guid> AddToPayments([FromBody] PaymentDTO payment)
        {
            _logger.LogInformation("AddToPayments requested for UserId: {UserId}, MessageId: {MessageId} at {Timestamp}", 
                payment.UserId, payment.MessageId, DateTime.UtcNow);
            try
            {
                var result = _paymentService.AddToPayments(payment);
                _logger.LogInformation("AddToPayments completed - Status: {Status}, PaymentId: {PaymentId}", 
                    result.status, result.data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AddToPayments for UserId: {UserId}", payment.UserId);
                throw;
            }
        }
    }
}
