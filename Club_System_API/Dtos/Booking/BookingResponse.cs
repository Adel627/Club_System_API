namespace Club_System_API.Dtos.Booking
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string? CoachName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = "Pending";
        public bool IsPaid { get; set; }
    }
}
