using Club_System_API.Abstractions;
using Club_System_API.Dtos.Membership;

namespace Club_System_API.Services
{
    public interface IMembershipService
    {

        Task<Result<MembershipResponse>> AddAsync(MembershipRequest request, CancellationToken cancellationToken);
        Task<Result<FeatureResponse>> AddFeatureAsync(int membershipid, FeatureRequest request, CancellationToken cancellationToken);

        Task<List<MembershipResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result> AssignToUserAsync(string userId, int membershipId);
        Task<Result> Cancel(string userid, CancellationToken cancellationToken);

        Task<Result<MembershipResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<string>> CreateStripeCheckoutSessionAsync(string userId, int membershipId, string domain);
        Task<Result> VerifyStripePaymentAsync(string sessionId);
        Task<Result<string>> CreateRenwalStripeCheckoutSessionAsync(string userId, string domain);
        Task<Result> VerifyRenwalStripePaymentAsync(string sessionId);


        Task<Result<MembershipResponse>> UpdateAsync(int id, UpdateMembershipRequest request, CancellationToken cancellationToken);

        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken);

    }
}
