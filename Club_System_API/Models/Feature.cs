namespace Club_System_API.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public Membership Membership { get; set; } = default!;
        public int MembershipId {  get; set; }
    }
}
