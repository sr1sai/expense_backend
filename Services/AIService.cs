using Domain;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AIService: IAIService
    {

        public Response<string> ClassifyMessage(MessageDTO message)
        {
            var response = new Response<string>
            {
                status = true,
                message = "Message classified successfully.",
                data = "Classified as General Inquiry"
            };
            return response;
        }
    }
}
