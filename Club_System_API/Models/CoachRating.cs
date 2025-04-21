namespace Club_System_API.Models
{
    public class CoachRating
    {
        public string UserId { get; set; }=string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;
        public int Rating { get; set; } 
        public DateTime RatedAt { get; set; } = DateTime.UtcNow;
    }
}
