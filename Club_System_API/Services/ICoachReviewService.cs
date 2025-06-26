using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Dtos.CoachReview;

namespace Club_System_API.Services
{
    public interface ICoachReviewService
    {
        Task<Result<ReviewCoachResponse>> AddAsync(string userid, ReviewCoachRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string userid, ReviewCoachRequest request, CancellationToken cancellationToken = default);
        Task<Result<List<CoachReviewWithUserImageResponse>>> GetAsync( int serviceid, CancellationToken cancellationToken = default);

    }
}
