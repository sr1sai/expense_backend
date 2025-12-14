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
    public class TransactionQueries
    {
        private readonly string _transactionTable;
        public TransactionQueries(string transactionTable)
        {
            _transactionTable = transactionTable;
        }
        public ParameterisedQuery GetTransactionsQuery(Guid userId)
        {
            var query = $@"SELECT * FROM ""{_transactionTable}"" WHERE ""UserId"" = @UserId";
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", userId }
            };
            return new ParameterisedQuery() { query = query, parameters = parameters };
        }

        public ParameterisedQuery AddTransactionQuery(TransactionDTO transaction)
        {
            var query = $@"
                INSERT INTO ""{_transactionTable}"" 
                ( ""UserId"", ""Type"", ""Amount"", ""Target"", ""Time"", ""MessageId"", ""Account"" ) 
                VALUES ( @UserId, @Type, @Amount, @Target, @Time, @MessageId, @Account)
                RETURNING ""Id"";
            ";
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", transaction.UserId },
                { "@Type", transaction.Type },
                { "@Amount", transaction.Amount },
                { "@Target", transaction.Target },
                { "@Time", transaction.Time },
                { "@MessageId", transaction.MessageId },
                { "@Account", transaction.Account },
            };
            return new ParameterisedQuery() { query = query, parameters = parameters };
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly TransactionQueries _transactionQueries;

        public TransactionRepository(IDatabaseContext databaseContext, IDatabaseConfig databaseConfig)
        {
            _databaseContext = databaseContext;
            _transactionQueries = new TransactionQueries(databaseConfig.Configuration["Tables:TransactionsTable"]);
        }

        public List<Transaction> GetTransactions(Guid userId)
        {
            try
            {
                var query = _transactionQueries.GetTransactionsQuery(userId);
                var result = _databaseContext.GetData<Transaction>(query);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving transactions", ex);
            }
        }

        public Guid AddTransaction(TransactionDTO transaction)
        {
            try
            {
                var query = _transactionQueries.AddTransactionQuery(transaction);
                var result = _databaseContext.ExecuteScalar(query);
                return Guid.Parse(result.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding transaction", ex);

            }
        }
    }
}
