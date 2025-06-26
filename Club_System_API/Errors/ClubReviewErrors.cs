using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class ClubReviewErrors
    {
        public static readonly Error ClubReviewNotFound =
         new("ClubReview.NotFound", "No ClubReview was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedReview =
         new("User.DuplicatedReview", "Another user with the same Id is already Review the Club", StatusCodes.Status409Conflict);

        public static readonly Error InvalidRate =
         new("User.InvalidRate", "the Rate Should be Less Than or Equal to 5", StatusCodes.Status400BadRequest);

    }
}
