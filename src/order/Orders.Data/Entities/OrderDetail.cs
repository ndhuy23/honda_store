using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.Entities
{
    public class OrderDetail : Base
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid ColorId { get; set; }

        public Guid ProductId {  get; set; }

        
        
        
    }
}
