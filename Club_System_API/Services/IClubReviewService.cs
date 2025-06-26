using Club_System_API.Abstractions;
using Club_System_API.Dtos.ClubReview;
using Club_System_API.Dtos.ServiceReview;

namespace Club_System_API.Services
{
    public interface IClubReviewService
    {
        Task<Result<ClubReviewResponse>> AddAsync(string userid, ClubReviewRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(string userid, ClubReviewRequest request, CancellationToken cancellationToken = default);
        Task<Result<List<ClubReviewWithUserImageResponse>>> GetAllAsync( CancellationToken cancellationToken = default);
    
}
}
