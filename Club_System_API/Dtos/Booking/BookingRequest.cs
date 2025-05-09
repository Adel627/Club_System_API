namespace Club_System_API.Dtos.Booking
{
    public class BookingRequest
    {
        public int ServiceId { get; set; }
        public int? CoachId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
