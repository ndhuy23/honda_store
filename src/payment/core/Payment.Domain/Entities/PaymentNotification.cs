using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Entities
{
    public class PaymentNotification
    {
        public string Id { get; set; }

        public string PaymentRefId { get; set; } = string.Empty;

        public DateTime? NotiDate { get; set; }

        public string? NotiContent { get; set; }

        public decimal NotiAmount { get; set; }

        public string? NotiMessage { get; set; }

        public string? NoSignature { get; set; }

        public string? NoPaymentId { get; set; }

        public string? NoMerchantId { get; set; }

        public string? NotiStatus { get; set; }

        public DateTime? NotiResData { get; set; }

        public string? NotiResMessage { get; set; }

        public string? NotiResHttpCode {  get; set; }
    }
}
