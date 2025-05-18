using Club_System_API.Abstractions;
using Club_System_API.Dtos.Booking;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}

