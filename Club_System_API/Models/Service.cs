namespace Club_System_API.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; }=string.Empty;
        public double AverageRating { get; set; }

        public byte[]? Image {  get; set; }
        public string? ImageContentType { get; set; }

        public DateOnly CreatedAt { get; set; }=DateOnly.FromDateTime(DateTime.Now);

        public ICollection<ServiceCoach> coaches { get; set; } = default!;
        public ICollection<ServiceReview> reviews { get; set; } = default!;
        public ICollection<Appointment> appointments { get; set; } = default!;

    }
}
