using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class CoachRatingConfiguration : IEntityTypeConfiguration<CoachRating>
    {
        public void Configure(EntityTypeBuilder<CoachRating> builder)
        {
            builder.HasKey(cu => new { cu.CoachId, cu.UserId });
           
        }
    }
}
