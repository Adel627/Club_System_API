using FluentValidation;

namespace Club_System_API.Dtos.ServiceCoach
{
    public class ServiceCoachRequestValidator:AbstractValidator<ServiceCoachRequest>
    {
        public ServiceCoachRequestValidator()
        {
            RuleFor(x => x.CoachId).NotEmpty();
            RuleFor(x => x.ServiceId).NotEmpty();

        }
    }
}
