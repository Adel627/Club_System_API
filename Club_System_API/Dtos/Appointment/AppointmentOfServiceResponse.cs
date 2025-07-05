namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentOfServiceResponse
    (
      int Id,
      int CoachId,
      DayOfWeek Day,
      TimeOnly? Time,
      int? Duration,
      int MaxAttenderNum,
      int CurrentAttenderNum
        );
}
