using AppServer.Business.Authentication;
using AppServer.Business.Service;
using AppShare.Models.Auth;
using AppShare.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace AppServer.ApiControllers
{
    [UserAuthorize]
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthApiController(IUserService userService, IAuthService authService, IMapper mapper)
        {
            _userService = userService;
            _authService = authService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "User đăng nhập", Description = "API này dùng để đăng nhập vào hệ thống, khi đăng nhập thành công sẽ trả về token cho client")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequestModel model)
        {
            var result = await _authService.Login(model);
            return Ok(result);
        }

        [SwaggerOperation(Summary = "User đăng nhập", Description = "API này dùng để đăng nhập vào hệ thống, khi đăng nhập thành công sẽ trả về token cho client")]
        [HttpGet("login/token")]
        public IActionResult Login()
        {
            var user = HttpContext.Items["User"];
            if (user is null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<UserModel>(user));
        }

        [AllowAnonymous]
        [HttpPut("recover-password")]
        [SwaggerOperation(Summary = "Lấy lại mật khẩu", Description = "API này dùng để lấy lại mật khẩu")]
        public async Task<IActionResult> RecoverPassword([FromQuery][EmailAddress] string email)
        {
            await _authService.RecoverPassword(email);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("reset-password")]
        [SwaggerOperation(Summary = "Tạo lại mật khẩu", Description = "API này dùng để tạo lại mật khẩu mới cho người dùng sau khi họ dùng chức năng <b>lấy lại mật khẩu</b>")]
        public async Task<IActionResult> ResetPassword([FromBody] AuthResetPasswordRequestModel model)
        {
            await _authService.ResetPassword(model);
            return Ok();
        }

        [AllowAnonymous]
        [SwaggerOperation(Summary = "User đăng ký", Description = "API này dùng để đăng ký tài khoản vào hệ thống, khi đăng ký thành công sẽ trả về id của client đó")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterRequestModel model)
        {
            var result = await _authService.Register(model);
            return Ok(result);
        }
    }
}
