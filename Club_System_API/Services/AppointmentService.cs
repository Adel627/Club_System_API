using Club_System_API.Abstractions;
using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.QA;
using Club_System_API.Dtos.ServiceCoach;
using Club_System_API.Errors;
using Club_System_API.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Club_System_API.Services
{
    public class AppointmentService(ApplicationDbContext context) : IAppointmentService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<AppointmentResponse>> AddAsync(AppointmentRequest request, CancellationToken cancellationToken = default)
        {

            var coach = await _context.Coachs.FindAsync(request.CoachId, cancellationToken);
            var service = await _context.Services.Include(s => s.coaches).FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);

            if (coach is null || service is null)
                return Result.Failure<AppointmentResponse>(ServiceCoachErrors.CoachorServiceNotFound);

            if (!service.coaches.Any(c => c.CoachId == request.CoachId))
            {
                return Result.Failure<AppointmentResponse>(AppointmentErrors.CoachNotAssigned);
            }
            var appointment = request.Adapt<Appointment>();
            await _context.AddAsync(appointment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(appointment.Adapt<AppointmentResponse>());
        }

        public async Task<IEnumerable<AppointmentOfServiceResponse>> GetAllOfServiceAsync(int serviceid,CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.Where(x => x.ServiceId == serviceid)
                .AsNoTracking()
           .ProjectToType<AppointmentOfServiceResponse>()
           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<AppointmentOfServiceResponse>> GetAllOfCoachAsync(int coachid, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments.Where(x => x.CoachId == coachid)
                            .AsNoTracking()
                       .ProjectToType<AppointmentOfServiceResponse>()
                       .ToListAsync(cancellationToken);
        }
      
        public async Task<IEnumerable<AppointmentOfServiceResponse>> GetAllAsync( CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .AsNoTracking()
           .ProjectToType<AppointmentOfServiceResponse>()
           .ToListAsync(cancellationToken);
        }
        public async Task<Result<AppointmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments.FindAsync(id, cancellationToken);

            return appointment is not null
                ? Result.Success(appointment.Adapt<AppointmentResponse>())
                : Result.Failure<AppointmentResponse>(AppointmentErrors.AppointmentNotFound);
        }

        public async Task<Result> UpdateAsync(int id, AppointmentUpdateRequest request, CancellationToken cancellationToken = default)
        {

            var currentAppointment = await _context.Appointments.FindAsync(id, cancellationToken);

            if (currentAppointment is null)
                return Result.Failure(AppointmentErrors.AppointmentNotFound);

           
            currentAppointment.Day = request.Day;
            currentAppointment.Time = request.Time;
            currentAppointment.MaxAttenderNum = request.MaxAttenderNum;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment is null)
                return Result.Failure(AppointmentErrors.AppointmentNotFound);

            _context.Remove(appointment);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

       
    }


}

