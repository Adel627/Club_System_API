using Microsoft.AspNetCore.Identity;

namespace Club_System_API.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string MembershipNumber {  get; set; } =string.Empty;
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }=string.Empty;
        public DateOnly Birth_Of_Date { get; set; }
        public DateOnly JoinedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? Renewal_date { get; set; }= DateOnly.FromDateTime(DateTime.Now).AddYears(1);
        public bool IsDisabled { get; set; }
        public byte[]? Image { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; } = [];
        public ICollection<CoachRating> CoachRating { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = default!;
    }
}
