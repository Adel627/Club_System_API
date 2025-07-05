using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public class MembershipErrors
    {
        public static readonly Error MembershipNotFound =
        new("Membership.NotFound", "No Membership was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error MembershipExpired =
        new("Membership.Expired", "You Should Renwal Your MemberShip", StatusCodes.Status417ExpectationFailed);

        public static readonly Error UserSubscriptionNotFound =
        new("UserSubscription.NotFound", "You Should subscripe in MemberShip First", StatusCodes.Status404NotFound);

        public static readonly Error CanNotRenwal =
        new("Renwal.NotAllowed", "You Can Renwal Your MemberShip in The Last 6 Month only", StatusCodes.Status405MethodNotAllowed);


    }
}
