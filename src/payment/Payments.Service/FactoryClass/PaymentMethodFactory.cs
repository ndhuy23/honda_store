using Microsoft.Extensions.DependencyInjection;
using Payments.Data.Dto;
using Payments.Data.Enum;
using Payments.Service.Interface;
using Payments.Service.Momo;
using Payments.Service.RabbitMQ.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.FactoryClass
{
    public class PaymentMethodFactory
    {
        public PaymentMethod _service;
        public IServiceProvider _serviceProvider { get; set; }
        public PaymentMethodFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public PaymentMethod getPaymentType(PaymentMethodType paymentType)
        {
            switch (paymentType)
            {
                case PaymentMethodType.MOMO:
                    return _serviceProvider.GetService<MomoService>();
                default:
                    throw new Exception("This bank type is unsupported");
            }
        }
    }
}
