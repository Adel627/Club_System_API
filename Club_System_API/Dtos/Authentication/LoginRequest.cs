namespace Club_System_API.Dtos.Authentication;
    public record LoginRequest(
        string PhoneNumber,
        string Password
    );