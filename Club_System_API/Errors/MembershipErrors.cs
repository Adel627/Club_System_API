using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public class MembershipErrors
    {
        public static readonly Error MembershipNotFound =
        new("Membership.NotFound", "No Membership was found with the given ID", StatusCodes.Status404NotFound);

    }
}
