using Stripe;

namespace Club_System_API.Dtos.Service
{
    public record TimeTableResponse
    (
        int Appoimentid,
        int CoachId,
        string CoachName,
        string day,
        TimeOnly time,
        string TrainingCategory,
        int Capacity,
        int Members,
        int? Duration

        );
    
}
