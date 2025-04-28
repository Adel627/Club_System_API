namespace Club_System_API.Services.Authentication
{
    public interface ITwilioService
    {
        Task SendVerificationCodeAsync(string phoneNumber);
        Task<bool> CheckVerificationCodeAsync(string phoneNumber, string code);
    }
}
