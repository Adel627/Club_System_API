using Club_System_API.Abstractions;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Dtos.ServiceReview;

namespace Club_System_API.Services
{
    public interface IServiceReviewService
    {
        Task<Result<ServiceReviewResponse>> AddAsync(string userid, ServiceReviewRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string userid, ServiceReviewRequest request, CancellationToken cancellationToken = default);
        Task<Result<List<ServiceReviewWithUserImageResponse>>> GetAsync( int serviceid, CancellationToken cancellationToken = default);
    }
}
