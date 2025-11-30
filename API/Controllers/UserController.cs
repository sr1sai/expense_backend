using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) 
        {
            _userService = userService;
        }
        [HttpGet]
        [ActionName("IsValidCredentials")]
        public Response<Guid> IsValidCredentials(string email, string password)
        {
            return _userService.IsValidCredentials(email, password);
        }
        [HttpPost]
        [ActionName("RegisterUser")]
        public Response<Guid> RegisterUser([FromBody] UsersDTO user)
        {
            return _userService.RegisterUser(user);
        }
        [HttpGet]
        [ActionName("GetUserById")]
        public Response<UserPublicDTO> GetUserById(Guid id)
        {
            return _userService.GetUserById(id);
        }
    }
}
