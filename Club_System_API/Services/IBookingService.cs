using Club_System_API.Abstractions;
using Club_System_API.Dtos.Booking;
using Club_System_API.Dtos.BookingPayment;

namespace Club_System_API.Services
{
    public interface IBookingService
    {
        Task<Result<BookingResponse>> BookAsync(string userId, int appointmentid);
        Task<Result<BookingPaymentResponse>> StartBookingPaymentAsync(string userId, int bookingId, string domain);
        Task<Result> VerifyStripePaymentAsync(string sessionId);
        Task<Result<List<BookingResponse>>> GetMyBookingsAsync(string userId);
        Task<Result> CancelAsync(string userId, int bookingId);
       // Task<Result<string>> CreateRenwalStripeCheckoutSessionAsync(string userId, string domain);
      //  Task<Result> VerifyRenwalStripePaymentAsync(string sessionId);

    }

}
