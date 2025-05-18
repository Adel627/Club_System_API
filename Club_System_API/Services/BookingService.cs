using Club_System_API.Abstractions;
using Club_System_API.Dtos.Booking;
using Club_System_API.Dtos.BookingPayment;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System;

namespace Club_System_API.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        private readonly StripeSettings _stripeSettings;
        private readonly StripeService _stripeService;

        public BookingService(ApplicationDbContext context, IOptions<StripeSettings> stripeOptions, StripeService stripeService)
        {
            _context = context;
            _stripeSettings = stripeOptions.Value;
            _stripeService = stripeService;
        }

        public async Task<Result<BookingResponse>> BookAsync(string userId, BookingRequest request)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Coach)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId);

            if (appointment == null)
                return Result.Failure<BookingResponse>(BookingErrors.AppointmentNotFound);

            if (appointment.CurrentAttenderNum >= appointment.MaxAttenderNum)
                return Result.Failure<BookingResponse>(BookingErrors.AppointmentFull);

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Result.Failure<BookingResponse>(UserErrors.UserNotFound);

            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddMinutes(60); // or based on service

            var booking = new Booking
            {
                UserId = userId,
                ServiceId = request.ServiceId,
                CoachId = request.CoachId,
                AppointmentId = request.AppointmentId,
                StartTime = startTime,
                EndTime = endTime,
                Status = BookingStatus.Pending,
            };

            _context.Bookings.Add(booking);
            appointment.CurrentAttenderNum++;
            await _context.SaveChangesAsync();

            return Result.Success(new BookingResponse
            {
                Id = booking.Id,
                UserName = user.UserName!,
                ServiceName = appointment.Service.Name,
                CoachName = appointment.Coach.FirstName,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Status = booking.Status.ToString(),
                IsPaid = booking.IsPaid
            });
        }

        public async Task<Result<List<BookingResponse>>> GetMyBookingsAsync(string userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.Coach)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var response = bookings.Select(b => new BookingResponse
            {
                Id = b.Id,
                UserName = b.User.UserName!,
                ServiceName = b.Service.Name,
                CoachName = b.Coach.FirstName,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status.ToString(),
                IsPaid = b.IsPaid
            }).ToList();

            return Result.Success(response);
        }

        public async Task<Result> CancelAsync(string userId, int bookingId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Appointment)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null)
                return Result.Failure(BookingErrors.BookingNotFound);

            if (booking.Status == BookingStatus.Cancelled)
                return Result.Failure(BookingErrors.CannotCancelBooking);

            booking.Status = BookingStatus.Cancelled;
            booking.Appointment.CurrentAttenderNum--;

            await _context.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result<BookingPaymentResponse>> StartBookingPaymentAsync(string userId, int bookingId, string domain)
        {
            var booking = await _context.Bookings
                .Include(b => b.Service)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null)
                return Result.Failure<BookingPaymentResponse>(BookingErrors.BookingNotFound);

            if (booking.IsPaid)
                return Result.Failure<BookingPaymentResponse>(BookingErrors.BookingNotFound);  //  ابقى زود ايررو واكتبه هنا انا مكسل

            var session = await _stripeService.CreateCheckoutSession(
                booking.Service.Price,
                $"Booking for {booking.Service.Name}",
                $"{domain}/booking-payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                $"{domain}/booking-payment-cancelled",
                new Dictionary<string, string>
                {
            { "bookingId", booking.Id.ToString() },
            { "userId", userId }
                });

            booking.StripeSessionId = session.Id;
            await _context.SaveChangesAsync();

            return Result.Success(new BookingPaymentResponse { StripeCheckoutUrl = session.Url });
        }

        public async Task HandleStripeWebhookAsync(string stripeEventJson, string stripeSignature)
        {
            var stripeEvent = EventUtility.ConstructEvent(
                stripeEventJson,
                stripeSignature,
                _stripeSettings.WebhookSecret);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session == null) return;

                var bookingId = int.Parse(session.Metadata["bookingId"]);

                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking is null) return;

                booking.IsPaid = true;
                await _context.SaveChangesAsync();
            }
        }
    }

}
