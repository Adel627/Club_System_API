namespace Club_System_API.Models
{
    public class Achievment
    {
        public int coachId { get; set; }
        public Coach coach { get; set; }
        public string Name { get; set; }=string.Empty;
    }
}
