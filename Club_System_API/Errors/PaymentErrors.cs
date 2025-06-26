using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class PaymentErrors
    {
        public static readonly Error PaymentNotComplete =
        new("Payment.NotComplete", "Payment not completed.", StatusCodes.Status400BadRequest);


        public static readonly Error PaymentNotFound =
       new("Payment.NotFound", "Payment not Found.", StatusCodes.Status404NotFound);

    }
}
