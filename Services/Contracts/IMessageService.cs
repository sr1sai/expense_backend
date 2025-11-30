using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IMessageService
    {
        Response<Guid> AddMessage(MessageDTO message);
    }
}
