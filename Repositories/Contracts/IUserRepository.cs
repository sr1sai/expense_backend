using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IUserRepository
    {
        Guid IsValidCredentials(string username, string password);
        Guid RegisterUser(UsersDTO user);
        UserPublicDTO GetUserById(Guid id);
    }
}
