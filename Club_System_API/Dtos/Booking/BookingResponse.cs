namespace Club_System_API.Dtos.Booking
{
    public class BookingResponse
    {
            public int Id { get; set; }
            public string UserName { get; set; } = default!;
            public string ServiceName { get; set; } = default!;
            public string CoachName { get; set; } = default!;
            public string Status { get; set; } = default!;
            public bool IsPaid { get; set; }
    } 
}