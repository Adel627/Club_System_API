namespace Club_System_API.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;
        public DayOfWeek Day { get; set; }
        public TimeOnly Time { get; set; }
        public int MaxAttenderNum { get; set; }
        public int CurrentAttenderNum { get; set; }
    }
}
