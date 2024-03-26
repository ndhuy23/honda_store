using MassTransit;
using MongoDB.Driver;
using Orders.Service.RabbitMQ.Messages;
using Products.Services.Core;
using ProductService.Data.DataAccess;
using ProductService.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Service.RabbitMQ.Consumer
{
    public class OrderCreateConsumer : IConsumer<OrderCreate>
    {
        public ProductDbContext _db;
        public OrderCreateConsumer(ProductDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<OrderCreate> context)
        {
            context.Message.Products.ForEach(p =>
            {
                var filter = Builders<Storage>.Filter.And(
                    Builders<Storage>.Filter.Eq(st => st.ProductId, p.ProductId),
                    Builders<Storage>.Filter.Eq(st => st.ColorId, p.ColorId));
                var product = _db.Storage.AsQueryable().First(st => st.ProductId == p.ProductId &&
                                                                    st.ColorId == p.ColorId);
                product.Quantity -= p.Quantity;
                var update = Builders<Storage>.Update.Set(st => st.Quantity, product.Quantity);
                _db.Storage.UpdateOne(filter, update);
            });
        }
    }
}
