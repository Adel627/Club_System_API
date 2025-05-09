using Club_System_API.Abstractions;
using Club_System_API.Dtos.Membership;
using Club_System_API.Errors;
using Club_System_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Club_System_API.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _context;

        public MembershipService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<MembershipResponse>> AddAsync(MembershipRequest request, CancellationToken cancellationToken)
        {
            var membership = new Membership
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DurationInDays = request.DurationInDays,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Memberships.AddAsync(membership, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new MembershipResponse
            {
                Id = membership.Id,
                Name = membership.Name,
                Description = membership.Description,
                Price = membership.Price,
                DurationInDays = membership.DurationInDays
            });
        }

        public async Task<List<MembershipResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Memberships
                .Select(m => new MembershipResponse
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    DurationInDays = m.DurationInDays
                }).ToListAsync(cancellationToken);
        }

        public async Task<Result> AssignToUserAsync(string userId, int membershipId)
        {
            var membership = await _context.Memberships.FindAsync(membershipId);
            if (membership == null)
                return Result.Failure(MembershipErrors.MembershipNotFound);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var userMembership = new UserMembership
            {
                ApplicationUserId = userId,
                MembershipId = membershipId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(membership.DurationInDays)
            };

            await _context.UserMemberships.AddAsync(userMembership);
            await _context.SaveChangesAsync();

            return Result.Success();
        }


    }
}
