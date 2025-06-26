using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class CoachReviewConfiguration : IEntityTypeConfiguration<CoachReview>
    {
        public void Configure(EntityTypeBuilder<CoachReview> builder)
        {
            builder.HasKey(cu => new { cu.CoachId, cu.UserId });
           
        }
    }
}
