using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Core;
using ProductService.Data.Dto;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        public ICompanyService _companyService;
        public ResultModel _result;
        public CompanyController(ICompanyService CompanyService)
        {
            _companyService = CompanyService;
            _result = new ResultModel();
        }
        [HttpPost]
        public IActionResult Post(string name)
        {
            _result = _companyService.Post(name);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
    }
}
