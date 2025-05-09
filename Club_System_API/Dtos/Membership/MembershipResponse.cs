namespace Club_System_API.Dtos.Membership
{
    public class MembershipResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
    }
}
