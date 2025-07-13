namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentRequest(
      int CoachId,
      int ServiceId,  
      string Day , 
      TimeOnly Time ,
    int? Duration,

      int MaxAttenderNum
     
    );
}
