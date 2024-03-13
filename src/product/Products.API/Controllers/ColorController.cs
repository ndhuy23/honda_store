using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Core;
using ProductService.Data.Dto;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        public IColorService _colorService;
        public ResultModel _result;
        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
            _result = new ResultModel();
        }
        [HttpPost("{name},{image}")]
        public async Task<IActionResult> Post(string name, string image)
        {
            _result = await _colorService.Post(name, image);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
        [HttpPost]
        public async Task<IActionResult> GetByIds(List<Guid> ids)
        {
            _result = await _colorService.GetByIds(ids);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
