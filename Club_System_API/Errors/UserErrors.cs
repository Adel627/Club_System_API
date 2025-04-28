using Club_System_API.Abstractions;

namespace Club_System_API.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
         new("User.InvalidCredentials", "Invalid PhoneNumber/password", StatusCodes.Status401Unauthorized);

        public static readonly Error DisabledUser =
            new("User.DisabledUser", "Disabled user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error LockedUser =
            new("User.LockedUser", "Locked user, please contact your administrator", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidJwtToken =
            new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidRefreshToken =
            new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedMembershipNumber =
            new("User.DuplicatedMembershipNumber", "Another user with the same MembershipNumber is already exists", StatusCodes.Status409Conflict);

        public static readonly Error DuplicatedPhoneNumber =
           new("User.DuplicatedPhoneNumber", "Another user with the same PhoneNumber is already exists", StatusCodes.Status409Conflict);

        public static readonly Error PhoneNumberNotConfirmed =
     new("User.PhoneNumberNotConfirmed", "PhoneNumber is not confirmed", StatusCodes.Status401Unauthorized);


        public static readonly Error InvalidCode =
            new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

        public static readonly Error UserNotFound =
        new("User.UserNotFound", "User is not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidRoles =
            new("Role.InvalidRoles", "Invalid roles", StatusCodes.Status400BadRequest);

    }
}
