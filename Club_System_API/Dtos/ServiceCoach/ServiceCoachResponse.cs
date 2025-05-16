namespace Club_System_API.Dtos.ServiceCoach
{
    public record ServiceCoachResponse(
        int CoachId,
          int ServiceId,
          DateOnly JoinedAtService
        );
}
