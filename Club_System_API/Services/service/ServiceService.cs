using Club_System_API.Abstractions;
using Club_System_API.Dtos.Service;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services.service
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;
        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ServiceResponse>> AddAsync([FromForm] ServiceRequest serviceRequest, CancellationToken cancellationToken = default)
        {
            var service = serviceRequest.Adapt<Service>(); 
           await _context.AddAsync(service,cancellationToken);
           await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(service.Adapt<ServiceResponse>());
        }

       

        public async Task<IEnumerable<ServiceResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Services
           .AsNoTracking()
           .ProjectToType<ServiceResponse>()
           .ToListAsync(cancellationToken);
        }

        public async Task<Result<ServiceResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var service = await _context.Services.FindAsync(id, cancellationToken);

            return service is not null
                ? Result.Success(service.Adapt<ServiceResponse>())
                : Result.Failure<ServiceResponse>(ServiceErrors.ServiceNotFound);
        }

        public async Task<Result> UpdateAsync(int id, ServiceRequest request, CancellationToken cancellationToken = default)
        {

            var currentService = await _context.Services.FindAsync(id, cancellationToken);

            if (currentService is null)
                return Result.Failure(ServiceErrors.ServiceNotFound);

            currentService.Name = request.Name;
            currentService.Price = request.Price;
            currentService.Description = request.Description;
            currentService.Image= FormFileExtensions.ConvertToBytes(request.Image);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
           var Service = await _context.Services.FindAsync(id);

            if (Service is null)
                return Result.Failure(ServiceErrors.ServiceNotFound);

             _context.Remove(Service);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }


    }
}
