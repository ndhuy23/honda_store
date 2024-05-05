using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Payments.Data.Dto;
using Payments.Data.Enum;
using Payments.Service.Core;
using Payments.Service.FactoryClass;
using Payments.Service.Interface;
using Payments.Service.Momo;
using Payments.Service.Momo.Config;
using RestSharp;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;



namespace Payments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        ResultModel _result;

        IServiceProvider _serviceProvider;
        IPaymentService _serviceCore;
        public PaymentController(IServiceProvider serviceProvider, IPaymentService serviceCore)
        {
            _result = new ResultModel();
            _serviceProvider = serviceProvider;
            _serviceCore = serviceCore;
        }

        [HttpPost]
        public async Task<ResultModel> CreatePaymentAsync(CreatePaymentDto createPaymentDto)
        {
            try
            {
                var paymentFactory = new PaymentMethodFactory(_serviceProvider);
                var service = paymentFactory.getPaymentType(createPaymentDto.PaymentType);
                _result.Data = await service.CreatePaymentAsync(createPaymentDto);
                _result.IsSuccess = true;
                _result.Message = "Create payment successful";
            }
            catch(Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }
        [HttpPut]
        public async Task<ResultModel> ChangeStatusOrder(Guid orderId)
        {
            try
            {
                _result = await _serviceCore.OrderIsPaid(orderId);
                _result.IsSuccess = true;
            }catch(Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }

    }
}
