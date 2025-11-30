using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [ActionName("AddMessage")]
        public Response<Guid> AddMessage(MessageDTO message)
        {
            return _messageService.AddMessage(message);
        }
    }
}
