using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Core;
using ProductService.Data.Dto;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IProductService _productService;
        ResultModel _result;
        public ProductController(IProductService productService)
        {
            _productService = productService;
            _result = new ResultModel();
        }
        [HttpPost("category")]
        public async Task<IActionResult> GetByCategory(GetProductByCategoryDto getProductDto)
        {
            _result = _productService.GetProductByCategory(getProductDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(PostProductDto postProductDto)
        {
            _result = await _productService.Post(postProductDto);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            _result = await _productService.GetById(id);
            if(!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
        [HttpGet]
        public async Task<IActionResult> Get(int pageIndex, int pageSize)
        {
            _result = await _productService.Get(pageIndex, pageSize);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
    }
}
