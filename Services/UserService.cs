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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public Response<Guid> IsValidCredentials(string email, string password)
        {
            Response<Guid> response = new Response<Guid>();
            try
            {
                response.status = true;
                response.data = _userRepository.IsValidCredentials(email, password);
                response.message = response.data == Guid.Empty? "Invalid Creds" : "Valid Creds";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = Guid.Empty;
            }
            return response;

        }
        public Response<Guid> RegisterUser(UsersDTO user)
        {
            Response<Guid> response = new Response<Guid>();
            try
            {
                Guid userId = _userRepository.RegisterUser(user);
                response.status = true;
                response.data = userId;
                response.message = "User registered successfully.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = Guid.Empty;
            }
            return response;
        }
        public Response<UserPublicDTO> GetUserById(Guid id)
        {
            Response<UserPublicDTO> response = new Response<UserPublicDTO>();
            try
            {
                UserPublicDTO user = _userRepository.GetUserById(id);
                response.status = true;
                response.data = user;
                response.message = "User fetched successfully.";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = new UserPublicDTO();
            }
            return response;
        }
    }
}
