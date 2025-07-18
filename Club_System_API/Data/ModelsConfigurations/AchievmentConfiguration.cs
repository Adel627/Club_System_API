using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class AchievmentConfiguration : IEntityTypeConfiguration<Achievment>
    {
        public void Configure(EntityTypeBuilder<Achievment> builder)
        {
            builder.HasKey(ca => new { ca.coachId, ca.Name });

        }
    }
}
