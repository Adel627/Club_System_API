using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class ServiceReviewErrors
    {
        public static readonly Error ServiceReviewNotFound =
         new("ServiceReview.NotFound", "No ServiceReview was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedReview =
         new("User.DuplicatedReview", "Another user with the same Id is already Review this Service", StatusCodes.Status409Conflict);

    }
}
