namespace Club_System_API.Dtos.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName,
    IFormFile? Image
);