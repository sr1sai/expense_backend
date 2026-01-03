using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger) 
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("IsValidCredentials")]
        public Response<Guid> IsValidCredentials(string email, string password)
        {
            _logger.LogInformation("IsValidCredentials requested for Email: {Email} at {Timestamp}", email, DateTime.UtcNow);
            try
            {
                var result = _userService.IsValidCredentials(email, password);
                _logger.LogInformation("IsValidCredentials completed for Email: {Email}, Status: {Status}, UserId: {UserId}", 
                    email, result.status, result.data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during IsValidCredentials for Email: {Email}", email);
                throw;
            }
        }

        [HttpPost]
        [ActionName("RegisterUser")]
        public Response<Guid> RegisterUser([FromBody] UsersDTO user)
        {
            _logger.LogInformation("RegisterUser requested for Email: {Email}, Name: {Name} at {Timestamp}", 
                user.Email, user.Name, DateTime.UtcNow);
            try
            {
                var result = _userService.RegisterUser(user);
                _logger.LogInformation("RegisterUser completed for Email: {Email}, Status: {Status}, UserId: {UserId}", 
                    user.Email, result.status, result.data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during RegisterUser for Email: {Email}", user.Email);
                throw;
            }
        }

        [HttpGet]
        [ActionName("GetUserById")]
        public Response<UserPublicDTO> GetUserById(Guid id)
        {
            _logger.LogInformation("GetUserById requested for UserId: {UserId} at {Timestamp}", id, DateTime.UtcNow);
            try
            {
                var result = _userService.GetUserById(id);
                _logger.LogInformation("GetUserById completed for UserId: {UserId}, Status: {Status}", id, result.status);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetUserById for UserId: {UserId}", id);
                throw;
            }
        }
    }
}
