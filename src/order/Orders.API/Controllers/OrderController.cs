using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orders.Data.Dto;
using Orders.Data.Response;
using Orders.Service.Core;

namespace Orders.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public IOrderService _orderService;
        public ResultModel _result;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            _result = new ResultModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateOrderDto order)
        {
            _result = await _orderService.CreateOrder(order);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid Id)
        {
            return Ok(_result);
        }
    }
}
