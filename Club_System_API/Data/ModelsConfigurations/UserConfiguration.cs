using Club_System_API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Club_System_API.Abstractions.Consts;

namespace Club_System_API.Data.ModelsConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .OwnsMany(x => x.RefreshTokens)
                .ToTable("RefreshTokens")
                .WithOwner()
                .HasForeignKey("UserId");



           // Default Data

        builder.HasData(new ApplicationUser
        {
            MembershipNumber = "Admin7",
            PhoneNumber= "01120443096",
            PhoneNumberConfirmed=true,
            Id = DefaultUsers.AdminId,
            FirstName = "Club System",
            LastName = "Admin",
            UserName = "Admin7",
            Birth_Of_Date = new DateOnly(2001, 12, 20),
            NormalizedUserName = "ADMIN7",
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            PasswordHash = "AQAAAAIAAYagAAAAEG11fXlkvcGxeUaPMcHd42YNq9TZKK3UfeGAXgrhHVQTIaDq50veaZghpOpmlPQ4cg=="
        });

        }
    }
}
