using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class ServiceReviewErrors
    {
        public static readonly Error ServiceReviewNotFound =
         new("ServiceReview.NotFound", "No ServiceReview was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedReview =
         new("User.DuplicatedReview", "Another user with the same Id is already Review this Service", StatusCodes.Status409Conflict);

        public static readonly Error InvalidRate =
       new("User.InvalidRate", "the Rate Should be Less Than or Equal to 5", StatusCodes.Status400BadRequest);

        public static readonly Error NotAllowedTOReview =
   new("User.NotAllowd", "You did not enrolled in this service yet", StatusCodes.Status405MethodNotAllowed);


    }
}
