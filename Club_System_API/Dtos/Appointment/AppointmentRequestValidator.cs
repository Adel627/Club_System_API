using FluentValidation;

namespace Club_System_API.Dtos.Appointment
{
    public class AppointmentRequestValidator:AbstractValidator<AppointmentRequest>
    {
        public AppointmentRequestValidator()
        {
            RuleFor(x => x.CoachId).NotEmpty();
            RuleFor(x => x.ServiceId).NotEmpty();
            RuleFor(x=> x.MaxAttenderNum).NotEmpty();
            RuleFor(x=> x.Day).NotEmpty();
            RuleFor(x=> x.Time).NotEmpty();
        }
    }
}
