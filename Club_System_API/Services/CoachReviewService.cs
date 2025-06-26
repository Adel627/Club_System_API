using Club_System_API.Abstractions;
using Club_System_API.Dtos.ClubReview;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Dtos.CoachReview;
using Club_System_API.Dtos.ServiceReview;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class CoachReviewService(ApplicationDbContext context) : ICoachReviewService
    {
        private readonly ApplicationDbContext _context=context;
        public async Task<Result<ReviewCoachResponse>> AddAsync(string userid, ReviewCoachRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Rating > 5)
                return Result.Failure<ReviewCoachResponse>(CoachErrors.InvalidRate);

            var coach = await _context.Coachs
               .SingleOrDefaultAsync(x => x.Id == request.CoachId);
            if(coach is null)
                return Result.Failure<ReviewCoachResponse>(CoachErrors.CoachNotFound);

            var exists = await _context.CoachReviews
             .AnyAsync(r => r.CoachId == request.CoachId && r.UserId == userid);           
            
            if (exists)
                return Result.Failure<ReviewCoachResponse>(CoachReviewErrors.DuplicatedReview);

            var coachrating = request.Adapt<CoachReview>();
            coachrating.UserId = userid;
            await _context.AddAsync(coachrating, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var ratings =  _context.CoachReviews
                .Where(x => x.CoachId == request.CoachId);
            coach.AverageRating =  ratings.Average(x => x.Rating);
           await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success(coachrating.Adapt<ReviewCoachResponse>());
        }

        public async Task<Result> UpdateAsync(string userid,ReviewCoachRequest request, CancellationToken cancellationToken = default)
        {


            if (request.Rating > 5)
                return Result.Failure<ReviewCoachResponse>(CoachErrors.InvalidRate);


            var coach = await _context.Coachs
              .SingleOrDefaultAsync(x => x.Id == request.CoachId);
            if (coach is null)
                return Result.Failure<ReviewCoachResponse>(CoachErrors.CoachNotFound);

            var currentCoachRate = await _context.CoachReviews
                .SingleOrDefaultAsync(r => r.CoachId == request.CoachId && r.UserId == userid);

            if (currentCoachRate is null)
                return Result.Failure(CoachReviewErrors.CoachReviewNotFound);

            currentCoachRate.CoachId = request.CoachId;
            currentCoachRate.Rating = request.Rating;
            await _context.SaveChangesAsync(cancellationToken);
            var ratings = _context.CoachReviews
                .Where(x => x.CoachId == request.CoachId);
            coach.AverageRating = ratings.Average(x => x.Rating);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<List<CoachReviewWithUserImageResponse>>> GetAsync(int coachid, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs.FindAsync(coachid);
            if (coach is null)
                return Result.Failure<List<CoachReviewWithUserImageResponse>>(CoachErrors.CoachNotFound);

            var reviews = await _context.CoachReviews
                .Include(r => r.User)
                .Where(r => r.CoachId == coachid)
                .Select(r => new CoachReviewWithUserImageResponse(
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
