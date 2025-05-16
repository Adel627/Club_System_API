using Club_System_API.Abstractions;
using Club_System_API.Dtos.Appointment;
using Club_System_API.Dtos.QA;
using Club_System_API.Dtos.ServiceCoach;
using Club_System_API.Errors;
using Club_System_API.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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



        //public async Task<IEnumerable<AppointmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        //{
        //    return await _context.QAs.OrderBy(x => x.SortNum)
        //   .AsNoTracking()
        //   .ProjectToType<AppointmentResponse>()
        //   .ToListAsync(cancellationToken);
        //}

        //public async Task<Result<AppointmentResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    var qa = await _context.QAs.FindAsync(id, cancellationToken);

        //    return qa is not null
        //        ? Result.Success(qa.Adapt<AppointmentResponse>())
        //        : Result.Failure<AppointmentResponse>(QAErrors.QANotFound);
        //}

        //public async Task<Result> UpdateAsync(int id, QARequest request, CancellationToken cancellationToken = default)
        //{

        //    var currentQA = await _context.QAs.FindAsync(id, cancellationToken);

        //    if (currentQA is null)
        //        return Result.Failure(QAErrors.QANotFound);

        //    var sortnum = GetSortNum();
        //    foreach (var x in sortnum)
        //    {
        //        if (request.SortNum == x)
        //            return Result.Failure<AppointmentResponse>(QAErrors.DuplicatedSortNum);
        //    }
        //    currentQA.Question = request.Question;
        //    currentQA.Answer = request.Answer;
        //    currentQA.SortNum = request.SortNum;
        //    await _context.SaveChangesAsync(cancellationToken);

        //    return Result.Success();
        //}
        //public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    var Service = await _context.QAs.FindAsync(id);

        //    if (Service is null)
        //        return Result.Failure(QAErrors.QANotFound);

        //    _context.Remove(Service);

        //    await _context.SaveChangesAsync(cancellationToken);

        //    return Result.Success();
        //}


        
    }


}

