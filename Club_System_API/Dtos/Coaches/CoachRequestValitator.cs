
using FluentValidation;

namespace Club_System_API.Dtos.Coaches
{
    public class CoachRequestValitator:AbstractValidator<CoachRequest>
    {
        public CoachRequestValitator()
        {
            RuleFor(x => x.FirstName)
             .NotEmpty()
             .Length(3, 100);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(3, 100);
            RuleFor(x => x.PhoneNumber).NotEmpty();

        }
    
    }
}
