using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.Dto.Response
{
    public class OrderResponse
    {
        public Guid UserId { get; set; }

        public Guid OrderId { get; set; }

        public long Amount { get; set; }

    }
}
