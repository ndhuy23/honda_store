﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.Dto
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }

        public List<ProductDetail> Products { get; set; }
    }
}
