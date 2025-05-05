using AppServer.Business.Authentication;
using AppServer.Business.Entities;
using AppServer.Business.Service;
using Microsoft.AspNetCore.Mvc;

namespace AppServer.ApiControllers
{
    [UserAuthorize]
    [Route("api/user")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAsync()
        {
            var results = await _userService.GetAsync();
            return Ok(results);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindByIdAsync([FromQuery] Guid id)
        {
            var result = await _userService.FindAsync(x => x.Id.Equals(id));
            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync([FromBody] UserEntity model)
        {
            var id = await _userService.CreateAsync(model);
            return Ok(id);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateAsync([FromBody] UserEntity model)
        {
            await _userService.UpdateAsync(model.Id, model);
            return Ok();
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}
