using FluentValidation;

namespace Club_System_API.Dtos.Authentication
{

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
                RuleFor(x => x.PhoneNumber)
                    .NotEmpty();
               

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}