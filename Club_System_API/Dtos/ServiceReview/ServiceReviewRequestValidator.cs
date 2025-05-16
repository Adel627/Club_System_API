using FluentValidation;

namespace Club_System_API.Dtos.ServiceReview
{
    public class ServiceReviewRequestValidator : AbstractValidator<ServiceReviewRequest>
    {
        public ServiceReviewRequestValidator()
        {
         RuleFor(x=> x.ServiceId).NotEmpty();   
         RuleFor(x=> x.Rating).NotEmpty();   
         RuleFor(x=> x.Review).NotEmpty();   
        }
    }
}
