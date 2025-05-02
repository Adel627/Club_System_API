using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.CoachRating;

namespace Club_System_API.Services
{
    public interface ICoachRatingService
    {
        Task<Result<RateCoachResponse>> AddAsync(string userid, RateCoachRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string userid, RateCoachRequest request, CancellationToken cancellationToken = default);
    }
}
