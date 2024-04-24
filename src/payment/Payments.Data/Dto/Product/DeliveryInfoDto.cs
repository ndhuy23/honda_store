using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Data.Dto.Product
{
    public class DeliveryInfoDto
    {
        public string? DeliveryAddress { get; set; }

        public string? DeliveryFee { get; set; }

        public string? Quantity { get; set; }
    }
}
