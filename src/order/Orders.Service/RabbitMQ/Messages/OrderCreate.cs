using Orders.Data.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Service.RabbitMQ.Messages
{

    public class OrderCreate
    {
        public List<ProductDetail> Products { get; set; }
    }
}
