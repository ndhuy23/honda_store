using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.Entities
{
    public class Order : Base
    {
        public Guid Id { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }

        public Guid UserId { get; set; }

        public bool IsPayment { get; set; }

        public Status Status {  get; set; } 

        public DateTime ExpectDeliveryDate { get; set;}

        public long Total { get; set; }
    }
}
