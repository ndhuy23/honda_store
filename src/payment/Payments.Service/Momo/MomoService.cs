using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Payments.Data.Dto;
using Payments.Service.FactoryClass;
using Payments.Service.Interface;
using Payments.Service.Momo.Config;
using Payments.Service.RabbitMQ.Messages;
using Payments.Service.RabbitMQ.Producers;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.Momo
{

    public class MomoService : PaymentMethod
    {
        ResultModel _result;
        PaymentSubmitedProducer _paymentSuccessfulProducer;
        MomoSetting _momoSetting;

        public MomoService(PaymentSubmitedProducer paymentSuccessfulProducer,IOptions<MomoSetting> momoSetting) {
            _result = new ResultModel();
            _paymentSuccessfulProducer = paymentSuccessfulProducer;
            _momoSetting = momoSetting.Value;
        }

        public async Task<ResultModel> CreatePaymentAsync(CreatePaymentDto payment)
        {

            var rawData = $"accessKey={_momoSetting.AccessKey}&amount={payment.Amount}" +
                $"&extraData=&ipnUrl={_momoSetting.IpnUrl}&orderId={payment.OrderId}" +
                $"&orderInfo={payment.OrderInfo}&partnerCode={_momoSetting.PartnerCode}" +
                $"&redirectUrl={_momoSetting.RedirectUrl}&requestId={payment.OrderId}" +
                $"&requestType={_momoSetting.RequestType}";

            var signature = ComputeHmacSha256(rawData, _momoSetting.SecretKey);
            var client = new RestClient("https://test-payment.momo.vn/v2/gateway/api/create");
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                partnerCode = _momoSetting.PartnerCode,
                requestId = payment.OrderId,
                amount = payment.Amount,
                orderId = payment.OrderId,
                orderInfo = payment.OrderInfo,
                redirectUrl = _momoSetting.RedirectUrl,
                ipnUrl = _momoSetting.IpnUrl,
                requestType = _momoSetting.RequestType,
                extraData = "",
                lang = payment.Lang,
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);
            _result.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);
            if (JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content)["resultCode"] != "0")
            {
                _result.IsSuccess = false;
                _result.Message = "";
                return _result;
            }
            _result.IsSuccess = true;
            _result.Message = "";

            return _result;
        }

        public async Task<ResultModel> PaymentSubmitedChangeStatusOrder(Guid orderId)
        {
            try
            {
                await _paymentSuccessfulProducer.SendEvent(new PaymentSubmited()
                {
                    OrderId = orderId
                });
                _result.IsSuccess = true;
                _result.Message = "";
            }catch(Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
        
    }
}
