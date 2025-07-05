using Club_System_API.Abstractions.Consts;
using FluentValidation;

namespace Club_System_API.Dtos.Authentication;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
      

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(x => x.PhoneNumber)
        .NotEmpty()
        .Matches(RegexPatterns.PhoneNumber)
        .WithMessage("Please enter a valid Egyptian mobile number starting with 010, 011, 012, or 015 and containing exactly 11 digits. The international format (+20) is not allowed.");


        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);
        
    }
}