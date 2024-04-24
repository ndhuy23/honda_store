using Payments.Data.Dto;
using Payments.Service.Momo.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.Interface
{
    public interface PaymentMethod
    {
        public Task<ResultModel> CreatePaymentAsync(CreatePaymentDto payment);

        public Task<ResultModel> PaymentSubmitedChangeStatusOrder(Guid orderId);
    }
}
