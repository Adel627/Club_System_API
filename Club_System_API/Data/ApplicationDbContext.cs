using Club_System_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    IdentityDbContext<ApplicationUser>(options)
{

    public DbSet<Service> Services { get; set; }
    public DbSet<Coach> Coachs { get; set; }
    public DbSet<ServiceCoach> ServiceCoaches { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<CoachRating> CoachRatings { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

   
}
