using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class ServiceCoachConfiguration : IEntityTypeConfiguration<ServiceCoach>
    {
        public void Configure(EntityTypeBuilder<ServiceCoach> builder)
        {
            builder.HasKey(sc => new { sc.ServiceId , sc.CoachId });

            
        }
    }
}
