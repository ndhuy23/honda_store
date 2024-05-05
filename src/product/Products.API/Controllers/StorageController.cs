using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Core;
using ProductService.Data.Dto;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        IStorageService _storageService;
        ResultModel _result;
        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
            _result = new ResultModel();
        }
        [HttpGet("{ProductId}/colors/{ColorId}")]
        public async Task<IActionResult> GetByProductIdAndColorId(Guid ProductId, Guid ColorId)
        {
            _result = await _storageService.GetByProductIdAndColorId(ProductId, ColorId);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            _result = await _storageService.GetByProductId(productId);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
    }
}
