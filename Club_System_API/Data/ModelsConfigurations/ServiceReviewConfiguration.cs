using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class ServiceReviewConfiguration : IEntityTypeConfiguration<ServiceReview>
    {
        public void Configure(EntityTypeBuilder<ServiceReview> builder)
        {
            builder.HasKey(su => new { su.ServiceId, su.UserId });
        }
    }
}
