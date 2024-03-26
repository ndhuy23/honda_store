using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.Dto
{
    public class ProductDetail
    {

        public Guid ProductId { get; set; }

        public Guid ColorId { get; set; }
        
        public int Quantity { get; set; }

        public long Price { get; set; }
        
    }
}
