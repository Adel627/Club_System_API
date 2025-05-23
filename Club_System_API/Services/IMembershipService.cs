﻿using Club_System_API.Abstractions;
using Club_System_API.Dtos.Membership;

namespace Club_System_API.Services
{
    public interface IMembershipService
    {
        Task<Result<MembershipResponse>> AddAsync(MembershipRequest request, CancellationToken cancellationToken);
        Task<List<MembershipResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result> AssignToUserAsync(string userId, int membershipId);
        Task<Result<MembershipResponse>> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Result<string>> CreateStripeCheckoutSessionAsync(string userId, int membershipId, string domain);
        Task<Result> AssignMembershipAfterPaymentAsync(string stripeSessionId);
    }
}
