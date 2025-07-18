namespace Club_System_API.Dtos.Appointment
{
    public record AppointmentResponse
    (
      int CoachId,
      int ServiceId,
      string Day , 
      TimeOnly? Time,
    int Duration,
    string? TrainingCategory,
      int MaxAttenderNum,
      int CurrentAttenderNum
    );
}
