namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentOfServiceResponse
    (
      int Id,
      int ServiceId,
      int CoachId,
      string Day,
      TimeOnly? Time,
      int? Duration,
      string? TrainingCategory,
      int MaxAttenderNum,
      int CurrentAttenderNum
        );
}
