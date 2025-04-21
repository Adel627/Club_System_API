using Club_System_API.Abstractions;
using Club_System_API.Dtos.Authentication;
using Club_System_API.Dtos.PhoneVerification;
using Club_System_API.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController (IAuthService authService,ITwilioService twilioService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITwilioService _twilioService = twilioService;

        [HttpPost("")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {

            var authResult = await _authService.GetTokenAsync(request.PhoneNumber, request.Password, cancellationToken);

            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
           
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
        }

        [HttpPost("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register( RegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RegisterAsync(request, cancellationToken);

            return result.IsSuccess ? Ok(result) : result.ToProblem();
        }


        [HttpPost("start")]
        public async Task<IActionResult> StartVerification([FromBody] PhoneRequest request)
        {
            await _twilioService.SendVerificationCodeAsync(request.PhoneNumber);
            return Ok(new { message = "Verification code sent." });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyRequest request)
        {
            var isVerified = await _twilioService.CheckVerificationCodeAsync(request.PhoneNumber, request.Code);

            if (!isVerified)
                return BadRequest(new { message = "Invalid verification code." });
         

            return Ok(new { message = "Phone number verified successfully." });
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            await _twilioService.SendMessageAsync(request.PhoneNumber, request.Message);
            return Ok(new { message = "Message sent successfully." });
        }


    }
}

