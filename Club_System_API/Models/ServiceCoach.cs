namespace Club_System_API.Models
{
    public class ServiceCoach
    {
        public int CoachId {  get; set; }
        public Coach Coach { get; set; }
        public int ServiceId {  get; set; }
        public Service Service { get; set; }
        public DateOnly JoinedAtService { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    }
}
