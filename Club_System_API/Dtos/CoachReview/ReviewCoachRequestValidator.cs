using FluentValidation;

namespace Club_System_API.Dtos.CoachRating
{
    public class ReviewCoachRequestValidator:AbstractValidator<ReviewCoachRequest>
    {
        public ReviewCoachRequestValidator()
        {
            RuleFor(x => x.CoachId).NotEmpty();
            RuleFor(x =>x.Rating).NotEmpty();
        }
    }
}
