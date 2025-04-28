using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class QAConfiguration : IEntityTypeConfiguration<QA>
    {
        public void Configure(EntityTypeBuilder<QA> builder)
        {
            builder.HasIndex(x => x.SortNum).IsUnique();
        }
    }
}
