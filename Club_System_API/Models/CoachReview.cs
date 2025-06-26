namespace Club_System_API.Models
{
    public class CoachReview
    {
        public string UserId { get; set; }=string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;
        public string Review { get; set; } = string.Empty;
        public int Rating { get; set; } 
        public DateTime ReviewAt { get; set; } = DateTime.UtcNow;
    }
}
