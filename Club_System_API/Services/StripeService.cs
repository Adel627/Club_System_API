using Club_System_API.Helper;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace Club_System_API.Services
{
    public class StripeService : IStripeService
    {
        private readonly StripeSettings _stripeSettings;

        public StripeService(IOptions<StripeSettings> stripeOptions)
        {
            _stripeSettings = stripeOptions.Value;
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        }

        public async Task<Session> CreateCheckoutSession(decimal amount, string name, string successUrl, string cancelUrl, Dictionary<string, string>? metadata = null)
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
                Metadata = metadata
            };

            var service = new SessionService();
            return await service.CreateAsync(options);
        }
    }
}