using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Booking;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(DefaultRoles.Member))]

    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpPost("{appointmentid}")]
        public async Task<IActionResult> Book([FromRoute] int appointmentid)
        {
            var userId = User.GetUserId();
            var result = await _bookingService.BookAsync(userId, appointmentid);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpPost("pay/{bookingId}")]
        public async Task<IActionResult> StartPayment(int bookingId)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var result = await _bookingService.StartBookingPaymentAsync(User.GetUserId()!, bookingId, domain);
            return result.IsSuccess ? Ok(new { redirectUrl = result.Value.StripeCheckoutUrl }) : result.ToProblem();
        }

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpGet("verify")]
        public async Task<IActionResult> Verify([FromQuery] string session_id)
        {
            var result = await _bookingService.VerifyStripePaymentAsync(session_id);
            if (result.IsSuccess)
                return Ok(result.IsSuccess);

            return BadRequest(result.ToProblem());
        }

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.GetUserId();
            var result = await _bookingService.GetMyBookingsAsync(userId);
            return Ok(result.Value);
        }

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> Cancel([FromRoute]int bookingId)
        {
            var userId = User.GetUserId();
            var result = await _bookingService.CancelAsync(userId, bookingId);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }


        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpGet("RenwalBooking/{id}")]
        public async Task<IActionResult> RenwalBooking([FromRoute] int id)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var result = await _bookingService.CreateRenwalStripeCheckoutSessionAsync(User.GetUserId()!,id, domain);

            return result.IsSuccess
                ? Ok(new { redirectUrl = result.Value })
                : result.ToProblem();
        }

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpGet("verifyRenwal")]
        public async Task<IActionResult> VerifyRenwalBooking([FromQuery] string session_id)
        {
            var result = await _bookingService.VerifyRenwalStripePaymentAsync(session_id);
            if (result.IsSuccess)
                return Ok(result.IsSuccess);

            return BadRequest(result.ToProblem());
        }

    }
}

