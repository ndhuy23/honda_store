using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Core;
using ProductService.Data.Dto;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferentialController : ControllerBase
    {
        public IPreferentialService _preferentialService;
        public ResultModel _result;
        public PreferentialController(IPreferentialService PreferentialService)
        {
            _preferentialService = PreferentialService;
            _result = new ResultModel();
        }
        [HttpPost]
        public IActionResult Post(string name)
        {
            _result = _preferentialService.Post(name);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
    }
}
