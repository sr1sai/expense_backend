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
    public class PaymentQueries
    {
        private readonly string _paymentsTable;
        public PaymentQueries(string paymentsTable)
        {
            _paymentsTable = paymentsTable;
        }
        public ParameterisedQuery AddToPaymentsQuery(PaymentDTO payment)
        {
            var query = $@"
                INSERT INTO ""{_paymentsTable}"" 
                ( ""UserId"", ""MessageId"" ) 
                VALUES (@UserId, @MessageId )
                RETURNING ""Id"";
            ";
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", payment.UserId },
                { "@MessageId", payment.MessageId },
            };
            return new ParameterisedQuery() { query = query, parameters = parameters };
        }
    }
    public class PaymentRepository: IPaymentRepository
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly PaymentQueries _paymentQueries;
        public PaymentRepository(IDatabaseContext databaseContext, IDatabaseConfig databaseConfig)
        {
            _databaseContext = databaseContext;
            _paymentQueries = new PaymentQueries(databaseConfig.Configuration["Tables:PaymentsTable"] ?? "Payment");
        }

        public Guid AddToPayments(PaymentDTO payment)
        {
            try
            {
                var query = _paymentQueries.AddToPaymentsQuery(payment);
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
