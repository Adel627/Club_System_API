namespace Club_System_API.Models
{
    public class Coach
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public DateOnly Birth_Of_Date { get; set; }
        public DateOnly Joined { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool IsDisabled { get; set; }
        public string PhoneNumber {  get; set; } = string.Empty;
        public int Salary {  get; set; }
        public byte[]? Image { get; set; }
        public ICollection<ServiceCoach> Services { get; set; } = default!;
        public ICollection<CoachRating> Rating { get; set; }= default!;

    }
}
