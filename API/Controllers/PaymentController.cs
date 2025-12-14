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
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost]
        [ActionName("AddToPayments")]
        public Response<Guid> AddToPayments([FromBody] PaymentDTO payment)
        {
            return _paymentService.AddToPayments(payment);
        }
    }
}
