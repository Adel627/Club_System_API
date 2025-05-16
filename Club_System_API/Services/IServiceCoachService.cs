using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.ServiceCoach;

namespace Club_System_API.Services
{
    public interface IServiceCoachService
    {
        Task<Result<ServiceCoachResponse>> AddAsync(ServiceCoachRequest Request, CancellationToken cancellationToken = default);
      //  Task<IEnumerable<CoachResponse>> GetAllAsync(CancellationToken cancellationToken = default);
      //  Task<Result<CoachResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
      //  Task<Result> UpdateAsync(int id, CoachRequest coachRequest, CancellationToken cancellationToken = default);
    }
}
