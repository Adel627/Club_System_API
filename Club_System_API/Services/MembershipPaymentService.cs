using Club_System_API.Abstractions;
using Club_System_API.Dtos.MembershipPayment;
using Club_System_API.Errors;
using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

namespace Club_System_API.Services
{
    public class MembershipPaymentService : IMembershipPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IStripeService _stripeService;

        public MembershipPaymentService(ApplicationDbContext context, IStripeService stripeService)
        {
            _context = context;
            _stripeService = stripeService;
        }

        public async Task<Result<MembershipPaymentResponse>> StartPurchaseAsync(string userId, MembershipPaymentRequest request)
        {
            var membership = await _context.Memberships.FindAsync(request.MembershipId);
            if (membership == null)
                return (Result<MembershipPaymentResponse>)Result.Failure(MembershipErrors.MembershipNotFound);

            var stripeSession = await _stripeService.CreateCheckoutSession(membership.Price, membership.Name, successUrl: "...", cancelUrl: "...");

            var purchase = new MembershipPayment
            {
                UserId = userId,
                MembershipId = membership.Id,
                StripeSessionId = stripeSession.Id,
                IsPaid = false
            };

            _context.MembershipPayments.Add(purchase);
            await _context.SaveChangesAsync();

            return Result.Success(new MembershipPaymentResponse { StripeCheckoutUrl = stripeSession.Url });
        }

        public async Task HandleStripeWebhookAsync(string stripeEventJson, string stripeSignature)
        {
            var stripeEvent = EventUtility.ConstructEvent(stripeEventJson, stripeSignature, "<YourStripeWebhookSecret>");

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;

                var purchase = await _context.MembershipPayments.FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);
                if (purchase != null)
                {
                    purchase.IsPaid = true;

                    // Optionally assign membership here
                    _context.UserMemberships.Add(new UserMembership
                    {
                        ApplicationUserId = purchase.UserId,
                        MembershipId = purchase.MembershipId,
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(1)
                    });

                    await _context.SaveChangesAsync();
                }
            }
        }
    }

}
