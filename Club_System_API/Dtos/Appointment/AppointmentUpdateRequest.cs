namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentUpdateRequest
    (
      DayOfWeek Day,
      TimeOnly Time,
      int Duration,
      int MaxAttenderNum
        );
}
