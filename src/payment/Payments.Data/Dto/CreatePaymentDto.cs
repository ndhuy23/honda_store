using Payments.Data.Dto.Product;
using Payments.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Payments.Data.Dto
{
    public class CreatePaymentDto
    {
        public string? StoreName { get; set; }

        public string? StoreId { get; set; }

        public Guid OrderId { get; set; }
        public string OrderInfo { get; set; }
        
        public long Amount { get; set; }

        public List<ProductInfoDto>? Items { get; set; } 

        public DeliveryInfoDto? DeliveryInfoDto { get; set; }

        public UserInfo userInfo { get; set; }

        public string? ReferenceId { get; set; }

        public bool? AutoCapture { get; set; }

        public string Lang { get; set; }

        public PaymentMethodType PaymentType { get; set; }
    }
}
