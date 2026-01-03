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
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost]
        [ActionName("AddMessage")]
        public Response<Guid> AddMessage(MessageDTO message)
        {
            _logger.LogInformation("AddMessage requested for UserId: {UserId}, Sender: {Sender}, Content: {ContentPreview} at {Timestamp}",
                message.UserId, message.Sender, message.MessageContent?.Substring(0, Math.Min(50, message.MessageContent?.Length ?? 0)), DateTime.UtcNow);
            try
            {
                var result = _messageService.AddMessage(message);
                _logger.LogInformation("AddMessage completed - Status: {Status}, MessageId: {MessageId}",
                    result.status, result.data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AddMessage for UserId: {UserId}", message.UserId);
                throw;
            }
        }
    }
}
