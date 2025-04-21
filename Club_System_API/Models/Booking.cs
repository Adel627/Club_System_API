using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string UserId { get; set; }=string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        public int CoachId { get; set; }
        public Coach Coach { get; set; } = default!;

        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = default!;
    }

}
