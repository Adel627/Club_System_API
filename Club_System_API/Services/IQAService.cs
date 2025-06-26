using Club_System_API.Abstractions;
using Club_System_API.Dtos.QA;
using Club_System_API.Dtos.Service;
using Club_System_API.Dtos.Users;

namespace Club_System_API.Services
{
    public interface IQAService
    {
        Task<Result<QAResponse>> AddAsync(string userid, QARequest QA, CancellationToken cancellationToken = default);

        Task<IEnumerable<GetQAResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<GetQAResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, QARequest QA, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

    }
}
