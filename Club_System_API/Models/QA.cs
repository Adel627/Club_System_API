namespace Club_System_API.Models
{
    public class QA
    {
        public int Id { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; }=string.Empty;    
        public int SortNum { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
    }
}
