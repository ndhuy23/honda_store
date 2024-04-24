using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Service.Momo.Config
{
    public class MomoSetting
    {

        public string MomoApiUrl { get; set; }

        public string SecretKey { get; set; }

        public string AccessKey { get; set; }

        public string RedirectUrl { get; set; }

        public string IpnUrl { get; set; }

        public string PartnerCode { get; set; }

        public string RequestType { get; set; }

    }
}
