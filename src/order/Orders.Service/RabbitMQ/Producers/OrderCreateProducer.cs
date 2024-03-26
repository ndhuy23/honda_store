using MassTransit;
using Microsoft.Extensions.Hosting;
using Orders.Service.RabbitMQ.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.RabbitMQ.Producers
{
    public class OrderCreateProducer
    {
        readonly IBus _bus;
        public OrderCreateProducer(IBus bus)
        {
            _bus = bus;
        }
        

        public async Task SendEvent(OrderCreate message){
            await _bus.Publish<OrderCreate>(message);
        }

    }
}
