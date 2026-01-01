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
    public class MessageQueries
    {
        private readonly string _messageTable;
        public MessageQueries(string messageTable)
        {
            _messageTable = messageTable;
        }

        public ParameterisedQuery AddMessageQuery(MessageDTO message)
        {
            return new ParameterisedQuery
            {
                query = $@"
                    INSERT INTO ""{_messageTable}"" 
                    (""UserId"", ""Sender"", ""MessageContent"", ""Time"" )
                    VALUES (@userId, @sender, @messageContent, @time) 
                    RETURNING ""Id"";
                ",
                parameters = new Dictionary<string, object>
                {
                    { "@UserId", message.UserId},
                    { "@sender", message.Sender},
                    { "@messageContent", message.MessageContent },
                    { "@time", message.Time},
                }
            };
        }

    }
    public class MessageRepository : IMessageRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly MessageQueries _messageQueries;
        public MessageRepository(IDatabaseContext databaseContext, IDatabaseConfig databaseConfig)
        {
            _databaseContext = databaseContext;
            _messageQueries = new MessageQueries(databaseConfig.Configuration["Tables:MessagesTable"] ?? "Message");
        }

        public Guid AddMessage(MessageDTO message)
        {
            try
            {
                var query = _messageQueries.AddMessageQuery(message);
                object guid = _databaseContext.ExecuteScalar(query);
                return Guid.Parse(guid?.ToString() ?? Guid.Empty.ToString());

            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }

    }
}
