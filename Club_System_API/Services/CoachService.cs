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
    public class CoachService : ICoachService
    {
        private readonly ApplicationDbContext _context;
        public CoachService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<CoachResponse>> AddAsync(CoachRequest request, CancellationToken cancellationToken = default)
        {
            var PhonenumberIsExist = await _context.Coachs.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (PhonenumberIsExist)
                return Result.Failure<CoachResponse>(CoachErrors.DuplicatedPhoneNumber);

            var coach = request.Adapt<CoachWithReviewsResponse>();
            await _context.AddAsync(coach, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(coach.Adapt<CoachResponse>());
        }
        public async Task<Result> AddAchievmentAsync(int coachid, AchievmentRequest request, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs.FindAsync(coachid, cancellationToken);
            if (coach is null)
                return Result.Failure(CoachErrors.CoachNotFound);

            if (_context.Achievment.Where(x => x.coachId == coachid).Any(x => x.Name == request.Achievment))
                return Result.Failure(CoachErrors.DuplicatedAchievment);

            var achivment = new Achievment { coachId = coachid, Name = request.Achievment };
            await _context.AddAsync(achivment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<IEnumerable<CoachResponse>> GetAllAsync( bool isadmin,CancellationToken cancellationToken = default)
        {
            if (isadmin)
            {
                return await _context.Coachs
               .AsNoTracking()
               .ProjectToType<CoachResponse>()
               .ToListAsync(cancellationToken);
            }

            return await _context.Coachs.Where(x => x.IsDisabled != true)
           .AsNoTracking()
           .ProjectToType<CoachResponse>()
           .ToListAsync(cancellationToken);
        }

        public async Task<Result<CoachWithReviewsResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs.Include(c => c.Rating)
                .ThenInclude(c => c.User)
                .Include(c=> c.achievments)
                .SingleOrDefaultAsync(x => x.Id == id);
             


            return coach is not null
                ? Result.Success(coach.Adapt<CoachWithReviewsResponse>())
                : Result.Failure<CoachWithReviewsResponse>(CoachErrors.CoachNotFound);
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
            currentCoach.Description = request.Description;
            currentCoach.Birth_Of_Date = request.Birth_Of_Date;
            currentCoach.PhoneNumber = request.PhoneNumber;
            currentCoach.Salary = request.Salary;
            if (request.Image is not null)
            {
                currentCoach.Image = FormFileExtensions.ConvertToBytes(request.Image);
                currentCoach.ImageContentType = request.Image.ContentType;
            }


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
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var coach = await _context.Coachs.FindAsync(id);

            if (coach is null)
                return Result.Failure(CoachErrors.CoachNotFound);

            _context.Remove(coach);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        
    }
}
