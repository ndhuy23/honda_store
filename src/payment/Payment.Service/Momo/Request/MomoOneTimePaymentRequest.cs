using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Payment.Service.Momo.Response;
using Payment.Service.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Payment.Service.Momo.Request
{
    public class MomoOneTimePaymentRequest
    {
        public string partnerCode { get; set; }

        public string requestId { get; set; }

        public long amount { get; set; }

        public Guid orderId { get; set; }

        public string orderInfo { get; set; }

        public string redirectUrl { get; set; }

        public string ipnUrl { get; set; }

        public string requestType { get; set; }

        public string extraData { get; set; }

        public string lang { get; set; }

        public string signature { get; set; }

        public void MakeSignature (string accessKey, string secretKey) 
        {
            var rawHash = "accessKey=" + accessKey +
                "&amount=" + this.amount +
                "&extraData=" + this.extraData +
                "&ipnUrl=" + this.ipnUrl +
                "&orderId=" + this.orderId +
                "&orderInfo=" + this.orderInfo +
                "&partnerCode=" + this.partnerCode +
                "&redirectUrl=" + this.redirectUrl +
                "&requestId=" + this.requestId +
                "&requestType=" + this.requestType;

            this.signature = HashHelper.HmacSHA256(rawHash, secretKey);
        }

        public (bool, string?) GetLink(string paymentUrl)
        {
            using HttpClient client = new HttpClient();
            var requestData = JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,

            });
            var requestContent = new StringContent (requestData, Encoding.UTF8,
                "application/json");
            var createPaymentLinkRes = client.PostAsync(paymentUrl, requestContent).Result;

            if (createPaymentLinkRes.IsSuccessStatusCode)
            {
                var responseContent = createPaymentLinkRes.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert
                            .DeserializeObject<MomoOneTimePaymentCreateLinkResponse>(responseContent);
                if(responseData.resultCode == "0")
                {
                    return (true, responseData.payUrl);
                }
                else
                {
                    return (false, responseData.message);
                }

            }
            else
            {
                return (false, createPaymentLinkRes.ReasonPhrase);
            }
        }
    }
}
