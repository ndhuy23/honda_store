using MassTransit;
using Payments.Service.RabbitMQ.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.RabbitMQ.Producers
{
    public class PaymentSubmitedProducer
    {
        readonly IBus _bus;
        public PaymentSubmitedProducer(IBus bus)
        {
            _bus = bus;
        }


        public async Task SendEvent(PaymentSubmited message)
        {
            await _bus.Publish<PaymentSubmited>(message);
        }
    }
}
