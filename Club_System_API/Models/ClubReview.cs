namespace Club_System_API.Models
{
    public class ClubReview
    {
        public int Id { get; set; }
        public string Review { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime ReviewAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }=string.Empty;
        public ApplicationUser User { get; set; } = default!;
    }
}
