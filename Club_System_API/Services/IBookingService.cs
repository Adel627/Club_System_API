using Club_System_API.Abstractions;
using Club_System_API.Dtos.Booking;
using Club_System_API.Dtos.BookingPayment;

namespace Club_System_API.Services
{
    public interface IBookingService
    {
        Task<Result<BookingResponse>> BookAsync(string userId, BookingRequest request);
        Task<Result<List<BookingResponse>>> GetMyBookingsAsync(string userId);
        Task<Result> CancelAsync(string userId, int bookingId);
        Task<Result<BookingPaymentResponse>> StartBookingPaymentAsync(string userId, int bookingId, string domain);
        Task HandleStripeWebhookAsync(string stripeEventJson, string stripeSignature);
    }

}
