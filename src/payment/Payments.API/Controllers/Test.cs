using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Payments.Service.Momo.Config;

namespace Payments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        MomoSetting _momoSetting;
        public Test(IOptions<MomoSetting> momoSetting)
        {
            _momoSetting = momoSetting.Value;
        }
        [HttpGet]
        public string GetMomoUrl()
        {
            return _momoSetting.MomoApiUrl;
        }
    }
}
