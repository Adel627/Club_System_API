using Club_System_API.Abstractions;
using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.Coaches;

namespace Club_System_API.Services
{
    public interface IAppointmentService
    {
        Task<Result<AppointmentResponse>> AddAsync(AppointmentRequest AppointmentRequest, CancellationToken cancellationToken = default);
        Task<IEnumerable<AppointmentOfServiceResponse>> GetAllOfServiceAsync(int serviceid,CancellationToken cancellationToken = default);
        Task<IEnumerable<AppointmentOfServiceResponse>> GetAllOfCoachAsync(int coachid,CancellationToken cancellationToken = default);
        Task<IEnumerable<AppointmentOfServiceResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Result<AppointmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id, AppointmentUpdateRequest AppointmentRequest, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
