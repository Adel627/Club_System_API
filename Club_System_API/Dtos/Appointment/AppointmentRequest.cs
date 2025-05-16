namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentRequest(
      int CoachId,
      int ServiceId,  
      DayOfWeek Day , 
      TimeOnly? Time  ,
      int MaxAttenderNum,
      int Id
    );
}
