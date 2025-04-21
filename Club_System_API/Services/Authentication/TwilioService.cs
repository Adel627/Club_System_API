using Club_System_API.Helper;
using Club_System_API.Services.Authentication;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;

public class TwilioService :ITwilioService
{
    private readonly TwilioSettings _settings;
    private readonly ApplicationDbContext _context;

    public TwilioService(IOptions<TwilioSettings> settings,ApplicationDbContext context)
    {
        _settings = settings.Value;
        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        _context = context;
    }

    public async Task SendVerificationCodeAsync(string phoneNumber)
    {
        await VerificationResource.CreateAsync(
            to: phoneNumber,
            channel: "sms",
            pathServiceSid: _settings.VerifyServiceSid
        );
    }

    public async Task<bool> CheckVerificationCodeAsync(string phoneNumber, string code)
    {
        var verificationCheck = await VerificationCheckResource.CreateAsync(
            to: phoneNumber,
            code: code,
            pathServiceSid: _settings.VerifyServiceSid
        );
        if(verificationCheck.Status == "approved")
        {
            var user = _context.Users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            user.PhoneNumberConfirmed = true;
            _context.SaveChanges();
        }
        return verificationCheck.Status == "approved";
    }

    public async Task SendMessageAsync(string phoneNumber, string message)
    {
        await MessageResource.CreateAsync(
            body: message,
            from: new Twilio.Types.PhoneNumber(_settings.FromPhoneNumber),
            to: new Twilio.Types.PhoneNumber(phoneNumber)
        );
    }

}
