using Stripe.Checkout;

namespace Club_System_API.Services
{
    public interface IStripeService
    {
        Task<Session> CreateCheckoutSession(decimal amount, string name, string successUrl, string cancelUrl, Dictionary<string, string>? metadata = null);
    }
}
