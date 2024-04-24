using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Data.Dto.Product
{
    public class ProductInfoDto
    {
        public Guid ProductId {  get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? Category {  get; set; }

        public string? ImageUrl { get; set; }

        public string? ManuFacturer { get; set; }

        public long Price { get; set; }

        public int Quantity { get; set; }

        public long TotalPrice { get; set; }

        public string Unit {  get; set; }

        public long TaxAmount { get; set; }
    }
}
