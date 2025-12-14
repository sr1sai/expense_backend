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
    public class PaymentService: IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }
        public Response<Guid> AddToPayments(PaymentDTO payment)
        {
            try
            {
                var paymentId = _paymentRepository.AddToPayments(payment);
                return new Response<Guid>
                {
                    status = paymentId != Guid.Empty,
                    message = paymentId != Guid.Empty? "Payment added successfully.": "Adding To Payments Failed",
                    data = paymentId
                };
            }
            catch (Exception ex)
            {
                return new Response<Guid>
                {
                    status = false,
                    message = $"An error occurred: {ex.Message}",
                    data = Guid.Empty
                };
            }
        }
    }
}
