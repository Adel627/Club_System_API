using Club_System_API.Abstractions;
using Club_System_API.Dtos.Service;
using Club_System_API.Models;

namespace Club_System_API.Services.service
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<ServiceWithAllInfoResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result< ServiceResponse>> AddAsync(ServiceRequest Service, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, ServiceRequest ServiceRequest, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
