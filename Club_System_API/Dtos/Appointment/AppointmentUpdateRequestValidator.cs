using FluentValidation;

namespace Club_System_API.Dtos.Appointment
{
    public class AppointmentUpdateRequestValidator:AbstractValidator<AppointmentUpdateRequest>  
    {
        public AppointmentUpdateRequestValidator()
        {
     
            RuleFor(x => x.MaxAttenderNum).NotEmpty();
            RuleFor(x => x.Day).NotEmpty();
            RuleFor(x => x.Time).NotEmpty();
        }
    }
}
