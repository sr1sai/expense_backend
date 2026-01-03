using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AIController: ControllerBase
    {
        private readonly IAIService _aIService;
        private readonly ILogger<AIController> _logger;

        public AIController(IAIService aIService, ILogger<AIController> logger)
        {
            _aIService = aIService;
            _logger = logger;
        }

        [HttpPost]
        public Response<string> ClassifyMessage([FromBody] MessageDTO message)
        {
            _logger.LogInformation("ClassifyMessage requested for UserId: {UserId}, Sender: {Sender}, Content: {ContentPreview} at {Timestamp}", 
                message.UserId, message.Sender, message.MessageContent?.Substring(0, Math.Min(50, message.MessageContent?.Length ?? 0)), DateTime.UtcNow);
            try
            {
                var result = _aIService.ClassifyMessage(message);
                _logger.LogInformation("ClassifyMessage completed - Status: {Status}, Classification: {Classification}", 
                    result.status, result.data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during ClassifyMessage for UserId: {UserId}", message.UserId);
                throw;
            }
        }
    }
}
