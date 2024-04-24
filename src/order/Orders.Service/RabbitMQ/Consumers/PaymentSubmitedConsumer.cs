using MassTransit;
using Orders.Data.DataAccess;
using Payments.Service.RabbitMQ.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.RabbitMQ.Consumers
{
    public class PaymentSubmitedConsumer : IConsumer<PaymentSubmited>
    {
        OrderDbContext _db;
        public PaymentSubmitedConsumer(OrderDbContext db)
        {
            _db = db;
        }
        public Task Consume(ConsumeContext<PaymentSubmited> context)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var order = _db.Orders.Where(od => od.Id == context.Message.OrderId).First();
                    order.IsPayment = true;
                    order.Status = Data.Entities.Status.LoadAccept;
                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }
                return Task.CompletedTask;
            }
                
        }
    }
}
