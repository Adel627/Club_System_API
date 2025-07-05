using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Booking;
using Club_System_API.Dtos.BookingPayment;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Microsoft.AspNetCore.Identity;
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
        private readonly IStripeService _stripeService;
        private readonly UserManager<ApplicationUser> _userManager; 

        public BookingService(ApplicationDbContext context, IOptions<StripeSettings> stripeOptions, IStripeService stripeService,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _stripeSettings = stripeOptions.Value;
            _stripeService = stripeService;
            _userManager = userManager;
        }

        public async Task<Result<BookingResponse>> BookAsync(string userId, int appointmentid)
        {
            var usermembership = await _context.UserMemberships
                .SingleOrDefaultAsync(x => x.ApplicationUser.Id == userId);
            if (usermembership.EndDate <= DateTime.UtcNow) 
            {
                
                await _userManager.RemoveFromRolesAsync(usermembership.ApplicationUser,[ nameof(DefaultRoles.Member)]);
                return Result.Failure<BookingResponse>(MembershipErrors.MembershipExpired);
            }
            var appointment = await _context.Appointments
                .Include(a => a.Coach)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == appointmentid);

            if (appointment == null)
                return Result.Failure<BookingResponse>(BookingErrors.AppointmentNotFound);

            if (appointment.CurrentAttenderNum >= appointment.MaxAttenderNum)
                return Result.Failure<BookingResponse>(BookingErrors.AppointmentFull);

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return Result.Failure<BookingResponse>(UserErrors.UserNotFound);

            

            var booking = new Booking
            {
                UserId = userId,
                AppointmentId = appointmentid,
                Status = BookingStatus.Pending,
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Result.Success(new BookingResponse
            {
                Id = booking.Id,
                UserName = user.UserName!,
                ServiceName = appointment.Service.Name,
                CoachName = appointment.Coach.FirstName +" "+ appointment.Coach.LastName,
                Status = booking.Status.ToString(),
            });
        }


        public async Task<Result<BookingPaymentResponse>> StartBookingPaymentAsync(string userId, int bookingId, string domain)
        {
            var booking = await _context.Bookings
                .Include(b => b.Appointment).Include(b=>b.Appointment.Service)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

            if (booking == null)
                return Result.Failure<BookingPaymentResponse>(BookingErrors.BookingNotFound);

            if (booking.IsPaid)
                return Result.Failure<BookingPaymentResponse>(BookingErrors.BookingPayedBefore);  

            var session = await _stripeService.CreateCheckoutSession(
                booking.Appointment.Service.Price,
                $"Booking for {booking.Appointment.Service.Name}",
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

        public async Task<Result> VerifyStripePaymentAsync(string sessionId)
        {

            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            if (session.PaymentStatus != "paid")
                return Result.Failure(PaymentErrors.PaymentNotComplete);

            var bookingId = int.Parse(session.Metadata["bookingId"]);

            var booking = await _context.Bookings.Include(x => x.Appointment)
                 .SingleOrDefaultAsync(p=> p.Id == bookingId);
            if (booking is null)
                return Result.Failure(BookingErrors.BookingNotFound);

            var userId = session.Metadata["userId"];

            // 1. تأكد إن الدفع موجود في قاعدة البيانات
            var payment = await _context.Bookings
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId && p.UserId == userId);

            if (payment == null)
                return Result.Failure(PaymentErrors.PaymentNotFound);

            if (payment.IsPaid)
                return Result.Success("✅ Payment already verified.");

            booking.IsPaid = true;
            booking.Appointment.CurrentAttenderNum++;
            await _context.SaveChangesAsync();
           

            return Result.Success("✅ Payment verified and Booking Occured.");
        }

       
        public async Task<Result<List<BookingResponse>>> GetMyBookingsAsync(string userId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Appointment)
                .Include(b => b.User)
                .Include(b => b.Appointment.Service)
                .Include(b => b.Appointment.Coach)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var response = bookings.Select(b => new BookingResponse
            {
                Id = b.Id,
                UserName = b.User.UserName!,
                ServiceName = b.Appointment. Service.Name,
                CoachName = b.Appointment.Coach.FirstName,
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
       
    }

}
