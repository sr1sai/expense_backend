using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IUserService
    {
        Response<Guid> IsValidCredentials(string email, string password);
        Response<Guid> RegisterUser(UsersDTO user);
        Response<UserPublicDTO> GetUserById(Guid id);
    }
}
