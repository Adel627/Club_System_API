namespace Club_System_API.Dtos.Authentication
{

    public record RefreshTokenRequest(
        string Token,
        string RefreshToken
    );
}