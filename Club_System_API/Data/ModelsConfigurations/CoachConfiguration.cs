using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class CoachConfiguration:IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.Property(x => x.Specialty).HasMaxLength(100);
            builder.Property(x => x.Bio).HasMaxLength(1500);
            builder.HasIndex(x => x.PhoneNumber).IsUnique();

        }
    }
}
