namespace Club_System_API.Models
{
    public class MembershipPayment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int MembershipId { get; set; }
        public Membership Membership { get; set; }

        public string StripeSessionId { get; set; }
        public bool IsPaid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
