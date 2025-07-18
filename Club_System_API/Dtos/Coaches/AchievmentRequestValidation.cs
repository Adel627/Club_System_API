using FluentValidation;

namespace Club_System_API.Dtos.Coaches
{
    public class AchievmentRequestValidation:AbstractValidator<AchievmentRequest>
    {
        public AchievmentRequestValidation()
        {
            RuleFor(x=>x.Achievment).NotNull().NotEmpty();   
        }
    }
}
