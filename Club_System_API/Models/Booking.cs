using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
    
namespace Club_System_API.Models
    {
        public class Booking
        {
            public int Id { get; set; }

            [Required]
            public string UserId { get; set; } = string.Empty;
            public ApplicationUser User { get; set; } = default!;

            [Required]
            public int ServiceId { get; set; }
            public Service Service { get; set; } = default!;

            [Required]
            public int CoachId { get; set; }
            public Coach Coach { get; set; } = default!;

            [Required]
            public int AppointmentId { get; set; }
            public Appointment Appointment { get; set; } = default!;

            [Required]
            public DateTime StartTime { get; set; } = DateTime.UtcNow;

            public DateTime EndTime { get; set; }

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
        Cancelled,
        Completed
    }
}