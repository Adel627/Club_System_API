using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class BookingRenewalBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BookingRenewalBackgroundService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var now = DateTime.UtcNow;

            var expiredBookings = await context.Bookings
                .Where(b => b.IsPaid && b.StartDate != null && b.StartDate.Value.AddMonths(1) <= now)
                .ToListAsync();


            foreach (var booking in expiredBookings)
            {
                booking.IsPaid = false;
                booking.Status = BookingStatus.Pending;
                booking.StripeSessionId = null;

            }

            await context.SaveChangesAsync();

            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }
}
