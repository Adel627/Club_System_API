namespace Club_System_API.Models
{
    public class UserMembership
    {
        public int Id { get; set; }

        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public int MembershipId { get; set; }
        public Membership Membership { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
