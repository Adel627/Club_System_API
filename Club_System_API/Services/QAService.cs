using Club_System_API.Abstractions;
using Club_System_API.Dtos.QA;
using Club_System_API.Dtos.Service;
using Club_System_API.Errors;
using Club_System_API.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Club_System_API.Services
{
    public class QAService(ApplicationDbContext context,UserManager<ApplicationUser> userManager) : IQAService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager=userManager;

        public async Task<Result<QAResponse>> AddAsync(string userid, QARequest Request, CancellationToken cancellationToken = default)
        {
            var sortnum = GetSortNum();
            foreach (var x in sortnum) 
            {
                if(Request.SortNum == x)
                 return   Result.Failure<QAResponse>(QAErrors.DuplicatedSortNum);
            }
            var qa = Request.Adapt<QA>();
            qa.UserId = userid;
            await _context.AddAsync(qa, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(qa.Adapt<QAResponse>());
        }



        public async Task<IEnumerable<QAResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.QAs
           .AsNoTracking()
           .ProjectToType<QAResponse>()
           .ToListAsync(cancellationToken);
        }

        public async Task<Result<QAResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var qa = await _context.QAs.FindAsync(id, cancellationToken);

            return qa is not null
                ? Result.Success(qa.Adapt<QAResponse>())
                : Result.Failure<QAResponse>(QAErrors.QANotFound);
        }

        public async Task<Result> UpdateAsync(int id, QARequest request, CancellationToken cancellationToken = default)
        {

            var currentQA = await _context.QAs.FindAsync(id, cancellationToken);

            if (currentQA is null)
                return Result.Failure(QAErrors.QANotFound);

            var sortnum = GetSortNum();
            foreach (var x in sortnum)
            {
                if (request.SortNum == x)
                  return  Result.Failure<QAResponse>(QAErrors.DuplicatedSortNum);
            }
            currentQA.Question = request.Question;
            currentQA.Answer = request.Answer;
            currentQA.SortNum = request.SortNum;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var Service = await _context.QAs.FindAsync(id);

            if (Service is null)
                return Result.Failure(QAErrors.QANotFound);

            _context.Remove(Service);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }


        private List<int> GetSortNum()
        {
            return _context.QAs.Select(x => x.SortNum).ToList();
        }


    }
}
