
using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class CoachRatingErrors
    {
        public static readonly Error CoachRateNotFound =
          new("CoachRate.NotFound", "No CoachRate was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedRate =
         new("User.DuplicatedRate", "Another user with the same Id is already Rate this Coach", StatusCodes.Status409Conflict);

    }
}
