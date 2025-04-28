using Club_System_API.Abstractions.Consts;
using Club_System_API.Abstractions;
using Club_System_API.Helper;
using Club_System_API.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Verify.V2.Service;
using Club_System_API.Models;

public class TwilioService :ITwilioService
{
    private readonly TwilioSettings _settings;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager ;

    public TwilioService(IOptions<TwilioSettings> settings,ApplicationDbContext context,
        UserManager<ApplicationUser> userManager
        )
    {

            _settings = settings.Value;
        TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        _context = context;
        _userManager = userManager;
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
            await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
            _context.SaveChanges();
        }
        //if (result.Succeeded)
        //{
        //    await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
        //    return Result.Success();
        //}
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
