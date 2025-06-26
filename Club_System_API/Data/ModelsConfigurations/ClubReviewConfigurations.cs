using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class ClubReviewConfigurations : IEntityTypeConfiguration<ClubReview>
    {
        public void Configure(EntityTypeBuilder<ClubReview> builder)
        {
           builder.HasIndex(x => x.UserId).IsUnique();
           builder.Property(x => x.UserId).IsRequired();
        }
    }
}
