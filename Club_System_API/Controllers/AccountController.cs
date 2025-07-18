using Club_System_API.Abstractions;
using Club_System_API.Dtos.Users;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{

    [Route("me")]
    [ApiController]
    [Authorize]
    public class AccountController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("")]
        public async Task<IActionResult> Info()
        {
          
            var result = await _userService.GetProfileAsync(User.GetUserId()!);

            return Ok(result.Value);
        }

        [HttpPut("info")]
        public async Task<IActionResult> InfoUpdate([FromForm] UpdateProfileRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            await _userService.UpdateProfileAsync(User.GetUserId()!, request);

            return NoContent();
        }

      

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
