using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Data.Response;
using Users.Data.ViewModels.Dtos;
using Users.Service.Core;

namespace Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserService _userService;
        public ResultModel _result;
        public UserController(IUserService userService)
        {
            _userService = userService;
            _result = new ResultModel();
        }
        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> RegisterAccount(RegisterUserDto user)
        {
            _result = await _userService.RegisterAccount(user);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> LoginAccount(LoginUserDto user)
        {
            _result = await _userService.LoginAccount(user);
            if (!_result.IsSuccess)
            {
                return BadRequest(_result);
            }
            return Ok(_result);
        }
        [HttpGet]
        public string Test()
        {
            _result.Data = "Test";
            _result.Message = " haha";
            return "haha";
        }
    }
}
