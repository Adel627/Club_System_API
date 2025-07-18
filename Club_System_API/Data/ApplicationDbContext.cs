using Club_System_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Coach> Coachs { get; set; }
    public DbSet<ServiceCoach> ServiceCoaches { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<CoachReview> CoachReviews { get; set; }
    public DbSet<QA> QAs { get; set; }
    public DbSet<ServiceReview> ServiceReviews { get; set; }
    public DbSet<ClubReview> clubReviews { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<MembershipPayment> MembershipPayments { get; set; }
    public DbSet<UserMembership> UserMemberships { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
     public DbSet<Achievment> Achievment { get; set; }
     public DbSet<Image> images { get; set; }
    public DbSet<Feature> Features { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ✅ فصل العلاقة بين ApplicationUser و RefreshToken (عشان ما تكونش Owned)
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.RefreshTokens)
            .WithOne(t => t.ApplicationUser)
            .HasForeignKey(t => t.ApplicationUserId)
            .IsRequired();

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
