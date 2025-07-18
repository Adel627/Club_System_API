using Azure;
using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Membership;
using Club_System_API.Errors;
using Club_System_API.Helper;
using Club_System_API.Models;
using Mapster;
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
            var membership = request.Adapt<Membership>();


            await _context.Memberships.AddAsync(membership, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = membership.Adapt<MembershipResponse>();
            return Result.Success(response);

        }

        public async Task<Result<FeatureResponse>> AddFeatureAsync(int membershipid, FeatureRequest request, CancellationToken cancellationToken)
        {
            var membership = await _context.Memberships.FindAsync(membershipid);
            if (membership == null)
                return Result.Failure<FeatureResponse>(MembershipErrors.MembershipNotFound);
            var feature = new Feature
            {
                MembershipId = membershipid,
                Name = request.features
            };
            await _context.AddAsync(feature, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = feature.Adapt<FeatureResponse>();
            return Result.Success(response);

        }

        public async Task<List<MembershipResponse>> GetAllAsync(CancellationToken cancellationToken)
        {

            var memberships = await _context.Memberships
            .Include(x => x.Features).AsNoTracking()
            .ToListAsync(cancellationToken);

            var response = memberships.Select(m => new MembershipResponse
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                DurationInDays = m.DurationInDays,
                ContentType = m.ImageContentType,
                Base64Data = m.Image != null ? Convert.ToBase64String(m.Image) : null,
                CreatedAt = m.CreatedAt,
                features = m.Features.Select(f => f.Name).ToList()
            }).ToList();

            return response;

        }

        public async Task<Result> AssignToUserAsync(string phonenumber, int membershipId)
        {
            var user = await _context.Users.FindAsync(phonenumber);
            if (user is not null)
                return Result.Failure<string>(MembershipErrors.NotAllowd);

            var membership = await _context.Memberships.FindAsync(membershipId);
            if (membership == null)
                return Result.Failure(MembershipErrors.MembershipNotFound);

            if (user == null)
                return Result.Failure(UserErrors.UserNotFound);

            var userMembership = new UserMembership
            {
                ApplicationUserId = user.Id,
                MembershipId = membershipId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(membership.DurationInDays)
            };

            await _context.UserMemberships.AddAsync(userMembership);
            await _context.SaveChangesAsync();

            await _userManager.AddToRoleAsync(user, nameof(DefaultRoles.Member));

            return Result.Success();
        }
        public async Task<Result<MembershipResponse>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null) return Result.Failure<MembershipResponse>(MembershipErrors.MembershipNotFound);

            return Result.Success(_mapper.Map<MembershipResponse>(membership));
        }

        public async Task<Result> Cancel(string userid,CancellationToken cancellationToken)
        {
            var UserMemberShip =await _context.UserMemberships
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userid);
            if (UserMemberShip is  null)
                return Result.Failure<string>(MembershipErrors.MembershipNotFound);

            _context.Remove(UserMemberShip);   
           await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<string>> CreateStripeCheckoutSessionAsync(string userId, int membershipId, string domain)
        {
            if(await _context.UserMemberships.AnyAsync(x=> x.ApplicationUserId == userId))
                return Result.Failure<string>(MembershipErrors.NotAllowd);

            var membership = await _context.Memberships.FindAsync(membershipId);

            if (membership == null)
                return Result.Failure<string>(MembershipErrors.MembershipNotFound);

            var options = new SessionCreateOptions
            {
                ClientReferenceId = userId,

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

                            //Images = string.IsNullOrWhiteSpace(membership.Image?.ToString())
                            //    ? null
                            //    : new List<string> { membership.Image.ToString() },
                            Name = membership.Name,
                            Description = membership.Description,

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

            var purchase = new MembershipPayment
            {
                UserId = userId,
                MembershipId = membership.Id,
                StripeSessionId = session.Id,
                IsPaid = false
            };
            _context.MembershipPayments.Add(purchase);
            await _context.SaveChangesAsync();

            return Result.Success(session.Url);
        }

        public async Task<Result> VerifyStripePaymentAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            if (session.PaymentStatus != "paid")
                return Result.Failure(PaymentErrors.PaymentNotComplete);

            var userId = session.ClientReferenceId;

            // 1. تأكد إن الدفع موجود في قاعدة البيانات
            var payment = await _context.MembershipPayments
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId && p.UserId == userId);

            if (payment == null)
                return Result.Failure(PaymentErrors.PaymentNotFound);

            if (payment.IsPaid)
                return Result.Success("✅ Payment already verified.");

            // 2. حدث حالة الدفع
            payment.IsPaid = true;

            var purchase = await _context.MembershipPayments.FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);
            var membership = await _context.Memberships
                .SingleOrDefaultAsync(m => m.Id == purchase.MembershipId);

            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            user.MembershipNumber = GenerateMembershipNumberExtensions.GenerateMembershipNumber();
            // Optionally assign membership here
            _context.UserMemberships.Add(new UserMembership
            {
                ApplicationUserId = purchase.UserId,
                MembershipId = purchase.MembershipId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(membership.DurationInDays)
            });

            await _context.SaveChangesAsync();

            await _userManager.AddToRoleAsync(user, nameof(DefaultRoles.Member));


            return Result.Success("✅ Payment verified and membership assigned.");
        }

        public async Task<Result<string>> CreateRenwalStripeCheckoutSessionAsync(string userId, string domain)
        {
            var usermembership = await _context.UserMemberships
                .SingleOrDefaultAsync(u => u.ApplicationUserId == userId);
            if (usermembership == null)
                return Result.Failure<string>(MembershipErrors.MembershipNotFound);
            if (DateTime.UtcNow.AddMonths(6) < usermembership.EndDate)
                return Result.Failure<string>(MembershipErrors.CanNotRenwal);

            var options = new SessionCreateOptions
            {
                ClientReferenceId = userId,

                LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmountDecimal = usermembership.Membership.Price * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Images = string.IsNullOrWhiteSpace(usermembership.Membership.Image?.ToString())
                                ? null
                                : new List<string> {usermembership.Membership.Image.ToString() },
                            Name = usermembership.Membership.Name,
                            Description = usermembership.Membership.Description,

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

            var purchase = new MembershipPayment
            {
                UserId = userId,
                MembershipId = usermembership.Membership.Id,
                StripeSessionId = session.Id,
                IsPaid = false
            };
            _context.MembershipPayments.Add(purchase);
            await _context.SaveChangesAsync();

            return Result.Success(session.Url);
        }

        public async Task<Result> VerifyRenwalStripePaymentAsync(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            if (session.PaymentStatus != "paid")
                return Result.Failure(PaymentErrors.PaymentNotComplete);

            var userId = session.ClientReferenceId;

            // 1. تأكد إن الدفع موجود في قاعدة البيانات
            var payment = await _context.MembershipPayments
                .FirstOrDefaultAsync(p => p.StripeSessionId == sessionId && p.UserId == userId);

            if (payment == null)
                return Result.Failure(PaymentErrors.PaymentNotFound);

            if (payment.IsPaid)
                return Result.Success("✅ Payment already verified.");

            // 2. حدث حالة الدفع
            payment.IsPaid = true;
            var usermembership = await _context.UserMemberships.Include(x => x.ApplicationUser)
                .Include(x => x.Membership)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (usermembership == null)
                return Result.Failure(MembershipErrors.MembershipNotFound);
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);

            if (DateTime.UtcNow < usermembership.EndDate)
            {
                var x = (usermembership.EndDate - DateTime.UtcNow).Days;
                usermembership.StartDate = DateTime.UtcNow;
                usermembership.EndDate = DateTime.UtcNow
               .AddDays(usermembership.Membership.DurationInDays)
               .AddDays(x);

                await _context.SaveChangesAsync();

                return Result.Success("✅ Payment verified and membership assigned.");
            }


            usermembership.StartDate = DateTime.UtcNow;
            usermembership.EndDate = DateTime.UtcNow
           .AddDays(usermembership.Membership.DurationInDays);

            await _context.SaveChangesAsync();

            await _userManager.AddToRoleAsync(user, nameof(DefaultRoles.Member));


            return Result.Success("✅ Payment verified and membership assigned.");
        }

        public async Task<Result<MembershipResponse>> UpdateAsync(int id, UpdateMembershipRequest request, CancellationToken cancellationToken)
        {
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
                return Result.Failure<MembershipResponse>(MembershipErrors.MembershipNotFound);

            membership.Name = request.Name;
            membership.Description = request.Description;
            membership.Price = request.Price;
            membership.DurationInDays = request.DurationInDays;
            membership.Image = FormFileExtensions.ConvertToBytes(request.Image);
            membership.ImageContentType = request.Image.ContentType;

           
            await _context.SaveChangesAsync(cancellationToken);
            var response = membership.Adapt<MembershipResponse>();

            return Result.Success(response);
        
        }


        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
                return Result.Failure(MembershipErrors.MembershipNotFound);

           
            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }





    }

}