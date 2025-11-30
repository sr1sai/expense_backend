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
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Response<Guid> AddMessage(MessageDTO message)
        {
            Response<Guid> response = new Response<Guid>();
            try
            {
                response.status = true;
                response.data = _messageRepository.AddMessage(message);
                response.message = response.data != Guid.Empty ? "Message Added to DB" : "Failed to add message";
            }
            catch(Exception ex)
            {
                response.status = false;
                response.data = Guid.Empty;
                response.message = ex.Message;
            }
            return response;
        }
    }
}
