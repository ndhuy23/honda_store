using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.RabbitMQ.Messages
{
    public class PaymentSubmited
    {
        public Guid OrderId { get; set; }

    }
}
