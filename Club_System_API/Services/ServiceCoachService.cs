using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.ServiceCoach;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class ServiceCoachService(ApplicationDbContext context) : IServiceCoachService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> AddCoachToServiceAsync(ServiceCoachRequest request, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs.FindAsync(request.CoachId,cancellationToken);
            var service = await _context.Services.Include(s => s.coaches).FirstOrDefaultAsync(s => s.Id == request.ServiceId,cancellationToken);

            if (coach is null || service is null)
                return Result.Failure<ServiceCoachResponse>(ServiceCoachErrors.CoachorServiceNotFound);

            if (service.coaches.Any(c => c.CoachId == request.CoachId))
                return Result.Failure<ServiceCoachResponse>(ServiceCoachErrors.AlreadyAssigned);
           ServiceCoach serviceCoach= new ServiceCoach 
           { CoachId = request.CoachId ,
            ServiceId=request.ServiceId
           };
            await _context.ServiceCoaches.AddAsync(serviceCoach,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> RemoveCoachFromServiceAsync(ServiceCoachRequest request, CancellationToken cancellationToken = default)
        {
            var service = await _context.Services.Include(s => s.coaches).FirstOrDefaultAsync(s => s.Id == request.ServiceId,cancellationToken);

            if (service is null)
                return Result.Failure<ServiceCoachResponse>(ServiceErrors.ServiceNotFound);

            var coach =  service.coaches.FirstOrDefault(c => c.CoachId == request.CoachId);
            if (coach is null)
                return Result.Failure<ServiceCoachResponse>(ServiceCoachErrors.CoachNotAssigned);

            service.coaches.Remove(coach);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        //public async Task<IEnumerable<CoachResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _context.Coachs
        //   .AsNoTracking()
        //   .ProjectToType<CoachResponse>()
        //    .ToListAsync(cancellationToken);
        //}

        //public async Task<Result<CoachResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    var coach = await _context.Coachs.FindAsync(id, cancellationToken);

        //    return coach is not null
        //        ? Result.Success(coach.Adapt<CoachResponse>())
        //        : Result.Failure<CoachResponse>(CoachErrors.CoachNotFound);
        //}

        //public async Task<Result> UpdateAsync(int id, CoachRequest request, CancellationToken cancellationToken = default)
        //{

        //    var currentCoach = await _context.Coachs.FindAsync(id, cancellationToken);

        //    if (currentCoach is null)
        //        return Result.Failure(CoachErrors.CoachNotFound);

        //    currentCoach.FirstName = request.FirstName;
        //    currentCoach.LastName = request.LastName;
        //    currentCoach.Specialty = request.Specialty;
        //    currentCoach.Bio = request.Bio;
        //    currentCoach.Birth_Of_Date = request.Birth_Of_Date;
        //    currentCoach.PhoneNumber = request.PhoneNumber;
        //    currentCoach.Salary = request.Salary;
        //    currentCoach.Image = FormFileExtensions.ConvertToBytes(request.Image);

        //    await _context.SaveChangesAsync(cancellationToken);

        //    return Result.Success();
        //}
        //public async Task<Result> ToggleStatus(int id)
        //{
        //    if (await _context.Coachs.FindAsync(id) is not { } coach)
        //        return Result.Failure(CoachErrors.CoachNotFound);

        //    coach.IsDisabled = !coach.IsDisabled;

        //    _context.Coachs.Update(coach);
        //    _context.SaveChanges();
        //    return Result.Success();


        //}
    }
}
