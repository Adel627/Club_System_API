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
            booking.Status=BookingStatus.Confirmed;
            booking.StartDate=DateTime.UtcNow;
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
                IsPaid = b.IsPaid,
                StartDate= b.StartDate, 
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
            if (booking.IsPaid)
            {
                booking.Appointment.CurrentAttenderNum--;
                booking.IsPaid = false;
            }
            await _context.SaveChangesAsync();
            return Result.Success();
        }


        public async Task<Result<string>> CreateRenwalStripeCheckoutSessionAsync(string userId, int bookingid, string domain)
        {
            var booking = await _context.Bookings
                .Include(b => b.Appointment)
                .Include(b => b.Appointment.Service)
                .FirstOrDefaultAsync(b => b.Id == bookingid);

           

            if (booking == null)
                return Result.Failure<string>(BookingErrors.BookingNotFound);

            //can renwal in last 10 days only 
            if (booking.StartDate != null && DateTime.UtcNow.AddDays(10) < booking.StartDate.Value.AddMonths(1))
                return Result.Failure<string>(BookingErrors.CanNotRenwal);

            if (booking.StartDate == null && booking.Appointment.CurrentAttenderNum >=booking.Appointment.MaxAttenderNum)
                return Result.Failure<string>(BookingErrors.AppointmentFull);


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

            return Result.Success(session.Url);
        }

        public async Task<Result> VerifyRenwalStripePaymentAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);



            if (session.PaymentStatus != "paid")
                return Result.Failure(PaymentErrors.PaymentNotComplete);

            var userId = session.Metadata["userId"];
            var bookingId = int.Parse(session.Metadata["bookingId"]);

            // 1. تأكد إن الدفع موجود في قاعدة البيانات
            var booking = await _context.Bookings.Include(b => b.Appointment)
            .Include(b => b.Appointment.Service)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

            if (booking == null)
                return Result.Failure(PaymentErrors.PaymentNotFound);

            if (booking.IsPaid)
                return Result.Success("✅ Payment already verified.");

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (booking.StartDate == null) 
            {

                booking.IsPaid = true;
                booking.Appointment.CurrentAttenderNum++;
                booking.Status = BookingStatus.Confirmed;
                booking.StartDate = DateTime.UtcNow;
            }else
            {
                var duration =  booking.StartDate.Value.AddMonths(1) - DateTime.UtcNow ;

                booking.IsPaid = true;
                booking.Status = BookingStatus.Confirmed;
                booking.StartDate = DateTime.UtcNow+duration;

            }
              await _context.SaveChangesAsync();
              return Result.Success("✅ Payment verified and Renwal completed");
        }


        



    }

}
