

using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class CoachErrors
    {
        public static readonly Error CoachNotFound =
         new("Coach.NotFound", "No Coach was found with the given ID", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedPhoneNumber =
         new("User.DuplicatedPhoneNumber", "Another user with the same PhoneNumber is already exists", StatusCodes.Status409Conflict);


        public static readonly Error InvalidRate =
       new("User.InvalidRate", "the Rate Should be Less Than or Equal to 5", StatusCodes.Status400BadRequest);

    }
}
