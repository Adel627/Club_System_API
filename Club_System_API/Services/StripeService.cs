using Stripe;
using Stripe.Checkout;

namespace Club_System_API.Services
{
    public class StripeService : IStripeService
    {
        public StripeService()
        {
            StripeConfiguration.ApiKey = "<Your Stripe Secret Key>";
        }

        public async Task<Session> CreateCheckoutSession(decimal amount, string name, string successUrl, string cancelUrl)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = amount * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = name,
                        },
                    },
                    Quantity = 1,
                }
            },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }
    }

}
