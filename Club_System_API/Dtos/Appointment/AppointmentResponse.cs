namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentResponse
    (
        int CoachId,
      int ServiceId,
      DayOfWeek Day , 
      TimeOnly? Time,
      int MaxAttenderNum,
       int CurrentAttenderNum
    );
}
