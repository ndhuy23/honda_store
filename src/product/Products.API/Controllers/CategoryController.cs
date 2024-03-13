using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Services.Core;
using ProductService.Data.Dto;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public ICategoryService _categoryService;
        public ResultModel _result;
        public CategoryController(ICategoryService categoryService) {
            _categoryService = categoryService;
            _result = new ResultModel();
        }
        [HttpPost]
        public IActionResult Post(string categoryName)
        {
            _result = _categoryService.Post(categoryName);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
    }
}
