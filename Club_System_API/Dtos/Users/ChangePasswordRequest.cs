namespace Club_System_API.Dtos.Users;

public record ChangePasswordRequest(
    string CurrentPassword,
    string NewPassword
);