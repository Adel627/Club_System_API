using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
          

            builder
              .HasOne(b => b.User)
              .WithMany()
              .HasForeignKey(b => b.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            builder
              .HasOne(b => b.Appointment)
              .WithMany()
              .HasForeignKey(b => b.AppointmentId)
              .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
