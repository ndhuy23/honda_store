using Payments.Data.Dto;
using Payments.Service.RabbitMQ.Producers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.Core
{
    public interface IPaymentService
    {
        public Task<ResultModel> OrderIsPaid(Guid orderId);
    }
    public class PaymentService : IPaymentService
    {
        ResultModel _result;
        PaymentSubmitedProducer _paymentSubmitedProducer;

        public PaymentService(PaymentSubmitedProducer paymentSubmitedProducer)
        {
            _result = new ResultModel();
            _paymentSubmitedProducer = paymentSubmitedProducer;
        }

        public async Task<ResultModel> OrderIsPaid(Guid orderId)
        {
            await _paymentSubmitedProducer.SendEvent(new RabbitMQ.Messages.PaymentSubmited()
            {
                OrderId = orderId
            });
            return _result;
        }
    }
}
