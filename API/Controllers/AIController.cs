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
        public AIController(IAIService aIService)
        {
            _aIService = aIService;
        }

        [HttpPost]
        public Response<string> ClassifyMessage([FromBody] MessageDTO message)
        {
            return _aIService.ClassifyMessage(message);
        }
    }
}
