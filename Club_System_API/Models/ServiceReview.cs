namespace Club_System_API.Models
{
    public class ServiceReview
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;
        public string Review {  get; set; }= string.Empty;
        public int Rating { get; set; }
        public DateTime ReviewAt { get; set; } = DateTime.UtcNow;
    }
}
