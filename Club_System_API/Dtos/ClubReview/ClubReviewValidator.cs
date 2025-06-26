using FluentValidation;

namespace Club_System_API.Dtos.ClubReview
{
    public class ClubReviewValidator:AbstractValidator<ClubReviewRequest>
    {
        public ClubReviewValidator()
        {
            RuleFor(x => x.Review).NotEmpty();
            RuleFor(x => x.Rating).NotEmpty();
        }
    }
}
