using Club_System_API.Abstractions;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Dtos.ServiceReview;
using Club_System_API.Errors;
using Club_System_API.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class ServiceReviewService(ApplicationDbContext context):IServiceReviewService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<ServiceReviewResponse>> AddAsync(string userid, ServiceReviewRequest request, CancellationToken cancellationToken = default)
        {

            var service = await _context.Services
               .SingleOrDefaultAsync(x => x.Id == request.ServiceId);
            if (service is null)
                return Result.Failure<ServiceReviewResponse>(ServiceErrors.ServiceNotFound);

            var exists = await _context.ServiceReviews
             .AnyAsync(r => r.ServiceId == request.ServiceId && r.UserId == userid);

            if (exists)
                return Result.Failure<ServiceReviewResponse>(ServiceReviewErrors.DuplicatedReview);

            var servicereview = request.Adapt<ServiceReview>();
            servicereview.UserId = userid;
            await _context.AddAsync(servicereview, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var reviews = _context.ServiceReviews
                .Where(x => x.ServiceId == request.ServiceId);
            service.AverageRating = reviews.Average(x => x.Rating);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(servicereview.Adapt<ServiceReviewResponse>());
        }

        public async Task<Result> UpdateAsync(string userid, ServiceReviewRequest request, CancellationToken cancellationToken = default)
        {
            var service = await _context.Services
              .SingleOrDefaultAsync(x => x.Id == request.ServiceId);
            if (service is null)
                return Result.Failure<ServiceReviewResponse>(ServiceErrors.ServiceNotFound);

            var currentservicereview = await _context.ServiceReviews
                .SingleOrDefaultAsync(r => r.ServiceId == request.ServiceId && r.UserId == userid);

            if (currentservicereview is null)
                return Result.Failure(ServiceReviewErrors.ServiceReviewNotFound);

            currentservicereview.ServiceId = request.ServiceId;
            currentservicereview.Rating = request.Rating;
            await _context.SaveChangesAsync(cancellationToken);
            var ratings = _context.ServiceReviews
                .Where(x => x.ServiceId == request.ServiceId);
            service.AverageRating = ratings.Average(x => x.Rating);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<List<ServiceReviewWithUserImageResponse>>> GetAsync(int serviceid, CancellationToken cancellationToken = default)
        {
            var service = await _context.Services.FindAsync(serviceid);
            if (service is null)
                return Result.Failure<List<ServiceReviewWithUserImageResponse>>(ServiceErrors.ServiceNotFound);

            var reviews = await _context.ServiceReviews
                .Include(r => r.User)
                .Where(r => r.ServiceId == serviceid)
                .Select(r => new ServiceReviewWithUserImageResponse(
                    r.User.Image,
                    r.Review,
                    r.Rating,
                    r.ReviewAt
                ))
                .ToListAsync(cancellationToken);

            return Result.Success(reviews);
        }


    }
}
