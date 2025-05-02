using FluentValidation;

namespace Club_System_API.Dtos.CoachRating
{
    public class RateCoachRequestValidator:AbstractValidator<RateCoachRequest>
    {
        public RateCoachRequestValidator()
        {
            RuleFor(x => x.CoachId).NotEmpty();
            RuleFor(x =>x.Rating).NotEmpty();
        }
    }
}
