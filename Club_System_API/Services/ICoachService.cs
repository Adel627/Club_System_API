using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;

namespace Club_System_API.Services
{
    public interface ICoachService
    {
        Task<IEnumerable<CoachResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<CoachResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<CoachResponse>> AddAsync(CoachRequest coachRequest, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, CoachRequest coachRequest, CancellationToken cancellationToken = default);
        Task<Result> ToggleStatus(int id);

    }
}
