using Database.Contracts;
using Domain;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserQueries
    {
        private readonly string _userTable;
        public UserQueries(string userTable)
        {
            _userTable = userTable;
        }

        public ParameterisedQuery LoginQuery(string email, string password)
        {
            var query = new ParameterisedQuery
            {
                query = $"SELECT \"Id\" FROM \"{_userTable}\" WHERE \"Email\" = @email AND \"Password\" = @password",
                parameters = new Dictionary<string, object>
                {
                    { "@email", email },
                    { "@password", password }
                }
            };
            return query;
        }
        public ParameterisedQuery RegisterQuery(UsersDTO user)
        {
            return new ParameterisedQuery
            {
                query = $@"
                    INSERT INTO ""{_userTable}"" 
                    (""Name"", ""Email"", ""Password"", ""PhoneNumber"")
                    VALUES (@name, @email, @password, @phone)
                    RETURNING ""Id"";
                ",
                parameters = new Dictionary<string, object>
                {
                    { "@name", user.Name },
                    { "@email", user.Email },
                    { "@password", user.Password },
                    { "@phone", (object?)user.PhoneNumber ?? DBNull.Value}
                }
            };
        }
        public ParameterisedQuery GetUserByIdQuery(Guid id)
        {
            return new ParameterisedQuery
            {
                query = $@"SELECT * FROM ""{_userTable}"" WHERE ""Id"" = @id",
                parameters = new Dictionary<string, object>
                {
                    { "@id", id }
                }
            };
        }


    }
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly UserQueries _userQueries;
        public UserRepository(IDatabaseContext databaseContext, IDatabaseConfig databaseConfig) 
        {
            _databaseContext = databaseContext;
            _userQueries = new UserQueries(databaseConfig.Configuration["Tables:UsersTable"] ?? "Users");
        }
        public Guid IsValidCredentials(string email, string password)
        {
            var users = _databaseContext.GetData<Users>(_userQueries.LoginQuery(email, password));
            return users.FirstOrDefault()?.Id ?? Guid.Empty;
        }
        public Guid RegisterUser(UsersDTO user)
        {
            try
            {
                var query = _userQueries.RegisterQuery(user);

                object result = _databaseContext.ExecuteScalar(query);
                
                return Guid.Parse(result?.ToString() ?? Guid.Empty.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Registration failed: " + ex.Message);
            }
        }
        public UserPublicDTO GetUserById(Guid id)
        {
            try
            {
                var query = _userQueries.GetUserByIdQuery(id);
                UserPublicDTO? user = _databaseContext.GetData<UserPublicDTO>(query).FirstOrDefault();
                return user ?? throw new Exception("User not found");
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to fetch user: " + ex.Message);
            }
        }
    }
}
