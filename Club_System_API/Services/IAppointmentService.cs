using Club_System_API.Abstractions;
using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.Coaches;

namespace Club_System_API.Services
{
    public interface IAppointmentService
    {
        Task<Result<AppointmentResponse>> AddAsync(AppointmentRequest AppointmentRequest, CancellationToken cancellationToken = default);
        //Task<IEnumerable<AppointmentResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        //Task<Result<AppointmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
        //Task<Result> UpdateAsync(int id, AppointmentRequest AppointmentRequest, CancellationToken cancellationToken = default);
        //Task<Result> DeleteAsync(int id);
    }
}
