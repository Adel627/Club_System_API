namespace Club_System_API.Models
{
    public class Membership
    {
        //public int Id { get; set; }
        //public string UserId { get; set; }
        //public ApplicationUser User { get; set; } = null!;
        //public string Type { get; set; } = "Yearly"; 
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
        public ICollection<UserMembership> UserMemberships { get; set; } = new List<UserMembership>();
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageContentType { get; set; }

        public ICollection<Feature> Features { get; set; }=new List<Feature>();
        public DateTime? CreatedAt { get; set; }=DateTime.UtcNow;
    }
} 
