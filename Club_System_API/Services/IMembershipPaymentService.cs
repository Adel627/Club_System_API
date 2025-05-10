using Club_System_API.Abstractions;
using Club_System_API.Dtos.MembershipPayment;

namespace Club_System_API.Services
{
    public interface IMembershipPaymentService
    {
        Task<Result<MembershipPaymentResponse>> StartPurchaseAsync(string userId, MembershipPaymentRequest request);
        Task HandleStripeWebhookAsync(string stripeEventJson, string stripeSignature);
    }
}
