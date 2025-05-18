using Club_System_API.Abstractions;
using Club_System_API.Dtos.Membership;
using Club_System_API.Errors;
using Club_System_API.Models;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace Club_System_API.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public MembershipService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<MembershipResponse>> AddAsync(MembershipRequest request, CancellationToken cancellationToken)
        {
            var membership = new Membership
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DurationInDays = request.DurationInDays,
                //CreatedAt = DateTime.UtcNow
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
        public async Task<Result<MembershipResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null) return Result.Failure<MembershipResponse>(MembershipErrors.MembershipNotFound);

            return Result.Success(_mapper.Map<MembershipResponse>(membership));
        }

        public async Task<Result<string>> CreateStripeCheckoutSessionAsync(string userId, int membershipId, string domain)
        {
            var membership = await _context.Memberships.FindAsync(membershipId);
            if (membership == null) return Result.Failure<string>(MembershipErrors.MembershipNotFound);

            var options = new SessionCreateOptions
            {
                CustomerEmail = (await _userManager.FindByIdAsync(userId))?.PhoneNumber,
                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = membership.Price * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Images=[membership.Image?.ToString()],
                            Name = membership.Name,
                            Description = membership.Description
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = $"{domain}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/payment-cancelled"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Result.Success(session.Url);
        }

        public async Task<Result> AssignMembershipAfterPaymentAsync(string sessionId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (session.PaymentStatus != "paid") return Result.Failure(MembershipErrors.MembershipNotFound);

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == session.CustomerEmail);

            if (user == null) return Result.Failure(UserErrors.UserNotFound);

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => session.LineItems.Any(li => li.Description == m.Description));

            if (membership == null) return Result.Failure(MembershipErrors.MembershipNotFound);

            user.MembershipId = membership.Id;
            user.MembershipStartDate = DateTime.UtcNow;
            user.MembershipEndDate = DateTime.UtcNow.AddDays(membership.DurationInDays);

            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }

}
