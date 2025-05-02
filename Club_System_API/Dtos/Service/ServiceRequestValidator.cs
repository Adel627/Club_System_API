using FluentValidation;

namespace Club_System_API.Dtos.Service
{
    public class ServiceRequestValidator:AbstractValidator<ServiceRequest>
    {
        public ServiceRequestValidator()
        {
            RuleFor(x => x.Name)
          .NotEmpty()
          .Length(3, 100);
 
        }
    }
}
