namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentUpdateRequest
    (
      string Day,
      TimeOnly Time,
      int Duration,
      int MaxAttenderNum
        );
}
