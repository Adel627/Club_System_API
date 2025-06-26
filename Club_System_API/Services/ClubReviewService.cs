using Club_System_API.Abstractions;
using Club_System_API.Dtos.ClubReview;
using Club_System_API.Errors;
using Club_System_API.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class ClubReviewService(ApplicationDbContext context):IClubReviewService
    {
        private readonly ApplicationDbContext _context=context;
        public async Task<Result<ClubReviewResponse>> AddAsync(string userid, ClubReviewRequest request, CancellationToken cancellationToken = default)
        {
            if(request.Rating>5)
                return Result.Failure<ClubReviewResponse>(ClubReviewErrors.InvalidRate);


            var exists = await _context.clubReviews
             .AnyAsync(r => r.UserId == userid);

            if (exists)
                return Result.Failure<ClubReviewResponse>(ClubReviewErrors.DuplicatedReview);

            var clubreview = request.Adapt<ClubReview>();
            clubreview.UserId = userid;
            await _context.AddAsync(clubreview, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success(clubreview.Adapt<ClubReviewResponse>());
        }

        public async Task<Result> UpdateAsync(string userid, ClubReviewRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Rating > 5)
                return Result.Failure<ClubReviewResponse>(ClubReviewErrors.InvalidRate);

            var currentClubReview = await _context.clubReviews
                .SingleOrDefaultAsync(r =>  r.UserId == userid);

            if (currentClubReview is null)
                return Result.Failure(ClubReviewErrors.ClubReviewNotFound);

            currentClubReview.Rating = request.Rating;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<List<ClubReviewWithUserImageResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
           
            var reviews = await _context.clubReviews
                .Include(r => r.User)
                .Select(r => new ClubReviewWithUserImageResponse(
                    r.User.Image,
                    r.User.FirstName,
                    r.User.LastName,
                    r.ReviewAt,
                    r.Rating,
                    r.Review
                ))
                .ToListAsync(cancellationToken);

            return Result.Success(reviews);
        }
    }
}
