using Domain;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AIService: IAIService
    {
        private readonly IAIRepository _aIRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITransactionRepository _transactionRepository;
        public AIService(IAIRepository aIRepository, IMessageRepository messageRepository, ITransactionRepository transactionRepository, IPaymentRepository paymentRepository)
        {
            _aIRepository = aIRepository;
            _messageRepository = messageRepository;
            _transactionRepository = transactionRepository;
            _paymentRepository = paymentRepository;
        }

        public Response<string> ClassifyMessage(MessageDTO message)
        {
            Guid saveResponse = _messageRepository.AddMessage(message); 
            
            string classification = _aIRepository.ClassifyMessage(message).ToString();
            
            if(classification == new MessageClassification().Payment)
            {
                Message Payemnet = new Message()
                {
                    Id = saveResponse,
                    UserId = message.UserId,
                    Sender = message.Sender,
                    MessageContent = message.MessageContent,
                    Time = message.Time
                };

                ExtractTransaction(Payemnet);
            }

            var response = new Response<string>
            {
                status = true,
                message = "Message classified successfully.",
                data = classification
            };
            return response;
        }

        private void ExtractTransaction(Message message)
        {
            PaymentDTO paymentDTO = new PaymentDTO()
            {
                UserId = message.UserId,
                MessageId = message.Id
            };
            _paymentRepository.AddToPayments(paymentDTO);

            //TransactionDTO transaction = _aIRepository.ExtractTransaction(message);

            //_transactionRepository.AddTransaction(transaction);

        }
    }
}
