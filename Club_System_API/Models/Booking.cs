using System.ComponentModel.DataAnnotations;
    
namespace Club_System_API.Models
    {
        public class Booking
        {
            public int Id { get; set; }
            public string UserId { get; set; } = string.Empty;
            public ApplicationUser User { get; set; } = default!;
            public int AppointmentId { get; set; }
            public Appointment Appointment { get; set; } = default!;

            public DateTime? StartDate {  get; set; }
            [Required]
            [MaxLength(20)]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? StripeSessionId { get; set; }

        public bool IsPaid { get; set; } = false;
        }
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
        
    }
}