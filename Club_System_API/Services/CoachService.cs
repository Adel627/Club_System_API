using Club_System_API.Abstractions;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class CoachService:ICoachService
    {
        private readonly ApplicationDbContext _context;
        public CoachService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<CoachResponse>> AddAsync(CoachRequest request, CancellationToken cancellationToken = default)
        {
            var PhonenumberIsExist= await _context.Coachs.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (PhonenumberIsExist)
               return Result.Failure<CoachResponse>(CoachErrors.DuplicatedPhoneNumber);

            var coach = request.Adapt<Coach>();
            await _context.AddAsync(coach, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(coach.Adapt<CoachResponse>());
        }

        public async Task<IEnumerable<CoachResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Coachs
           .AsNoTracking()
           .ProjectToType<CoachResponse>()
           .ToListAsync(cancellationToken);
        }

        public async Task<Result<CoachResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs.FindAsync(id, cancellationToken);

            return coach is not null
                ? Result.Success(coach.Adapt<CoachResponse>())
                : Result.Failure<CoachResponse>(CoachErrors.CoachNotFound);
        }

        public async Task<Result> UpdateAsync(int id, CoachRequest request, CancellationToken cancellationToken = default)
        {

            var currentCoach = await _context.Coachs.FindAsync(id, cancellationToken);

            if (currentCoach is null)
                return Result.Failure(CoachErrors.CoachNotFound);

               currentCoach.FirstName = request.FirstName;
            currentCoach.LastName = request.LastName;
            currentCoach.Specialty = request.Specialty;
            currentCoach.Bio=request.Bio;
            currentCoach.Birth_Of_Date = request.Birth_Of_Date;
            currentCoach.PhoneNumber = request.PhoneNumber;
            currentCoach.Salary = request.Salary;
            currentCoach.Image=FormFileExtensions.ConvertToBytes(request.Image);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> ToggleStatus(int id)
        {
            if (await _context.Coachs.FindAsync(id) is not { } coach)
                return Result.Failure(CoachErrors.CoachNotFound);

            coach.IsDisabled = !coach.IsDisabled;

           _context.Coachs.Update(coach);
            _context.SaveChanges();
            return Result.Success();


        }

        public async Task<Result> AssignServiceToCoach(int coachId, int serviceId)
        {
            var coachService = new Models.ServiceCoach { CoachId = coachId, ServiceId = serviceId };
             await  _context.ServiceCoaches.AddAsync(coachService);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
