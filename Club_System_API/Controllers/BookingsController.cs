using Club_System_API.Abstractions;
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
    public class BookingsController(IBookingService bookingService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Book([FromBody] BookingRequest request)
        {
            var userId = User.GetUserId();
            var result = await _bookingService.BookAsync(userId, request);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.GetUserId();
            var result = await _bookingService.GetMyBookingsAsync(userId);
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> Cancel(int bookingId)
        {
            var userId = User.GetUserId();
            var result = await _bookingService.CancelAsync(userId, bookingId);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [Authorize]
        [HttpPost("pay/{bookingId}")]
        public async Task<IActionResult> StartPayment(int bookingId)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var result = await _bookingService.StartBookingPaymentAsync(User.GetUserId()!, bookingId, domain);
            return result.IsSuccess ? Ok(new { redirectUrl = result.Value.StripeCheckoutUrl }) : result.ToProblem();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            try
            {
                await _bookingService.HandleStripeWebhookAsync(json, stripeSignature!);
                return Ok();
            }
            catch (StripeException ex)
            {
                return BadRequest(new { message = $"Stripe webhook error: {ex.Message}" });
            }
        }
    }
}

