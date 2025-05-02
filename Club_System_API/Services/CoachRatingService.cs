using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class CoachRatingService(ApplicationDbContext context) : ICoachRatingService
    {
        private readonly ApplicationDbContext _context=context;
        public async Task<Result<RateCoachResponse>> AddAsync(string userid, RateCoachRequest request, CancellationToken cancellationToken = default)
        {

            var coach = await _context.Coachs
               .SingleOrDefaultAsync(x => x.Id == request.CoachId);
            if(coach is null)
                return Result.Failure<RateCoachResponse>(CoachErrors.CoachNotFound);

            var exists = await _context.CoachRatings
             .AnyAsync(r => r.CoachId == request.CoachId && r.UserId == userid);           
            
            if (exists)
                return Result.Failure<RateCoachResponse>(CoachRatingErrors.DuplicatedRate);

            var coachrating = request.Adapt<CoachRating>();
            coachrating.UserId = userid;
            await _context.AddAsync(coachrating, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var ratings =  _context.CoachRatings
                .Where(x => x.CoachId == request.CoachId);
            coach.AverageRating =  ratings.Average(x => x.Rating);
           await _context.SaveChangesAsync(cancellationToken);
            
            return Result.Success(coachrating.Adapt<RateCoachResponse>());
        }

        public async Task<Result> UpdateAsync(string userid,RateCoachRequest request, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs
              .SingleOrDefaultAsync(x => x.Id == request.CoachId);
            if (coach is null)
                return Result.Failure<RateCoachResponse>(CoachErrors.CoachNotFound);

            var currentCoachRate = await _context.CoachRatings
                .SingleOrDefaultAsync(r => r.CoachId == request.CoachId && r.UserId == userid);

            if (currentCoachRate is null)
                return Result.Failure(CoachRatingErrors.CoachRateNotFound);

            currentCoachRate.CoachId = request.CoachId;
            currentCoachRate.Rating = request.Rating;
            await _context.SaveChangesAsync(cancellationToken);
            var ratings = _context.CoachRatings
                .Where(x => x.CoachId == request.CoachId);
            coach.AverageRating = ratings.Average(x => x.Rating);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
