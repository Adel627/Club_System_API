using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Club_System_API.Abstractions;
using Club_System_API.Dtos.Membership;
using Club_System_API.Extensions;
using Club_System_API.Helper;
using Club_System_API.Services;
using Club_System_API.Dtos.MembershipPayment;
using Microsoft.EntityFrameworkCore;
using Club_System_API.Abstractions.Consts;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipService _membershipService;
        private readonly StripeSettings _stripeSettings;

        public MembershipsController(
            IMembershipService membershipService,
            IOptions<StripeSettings> stripeOptions)
        {
            _membershipService = membershipService;
            _stripeSettings = stripeOptions.Value;

        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] MembershipRequest request, CancellationToken cancellationToken)
        {
            var result = await _membershipService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost("features/{membershipid}")]
        public async Task<IActionResult> Add([FromRoute] int membershipid, [FromBody] FeatureRequest request, CancellationToken cancellationToken)
        {
            var result = await _membershipService.AddFeatureAsync(membershipid, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _membershipService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        /// Assign a membership directly to the authenticated user (no payment).
        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost("assign/{phonenumber}/{membershipId}")]
        public async Task<IActionResult> AssignMembership([FromRoute] string phonenumber, [FromRoute] int membershipId)
        {
            var result = await _membershipService.AssignToUserAsync(phonenumber, membershipId);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        // Create Stripe Checkout Session to pay for selected membership.
        [Authorize]
        [HttpPost("select")]
        public async Task<IActionResult> SelectMembership([FromBody] MembershipPaymentRequest request)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var result = await _membershipService.CreateStripeCheckoutSessionAsync(User.GetUserId()!, request.MembershipId, domain);

            return result.IsSuccess
                ? Ok(new { redirectUrl = result.Value })
                : result.ToProblem();
        }
        [Authorize]
        [HttpGet("verifyselect")]
        public async Task<IActionResult> VerifyMembership([FromQuery] string session_id)
        {
            var result = await _membershipService.VerifyStripePaymentAsync(session_id);
            if (result.IsSuccess)
                return Ok(result.IsSuccess);

            return BadRequest(result.ToProblem());
        }

        [Authorize]
        [HttpGet("Renwal")]
        public async Task<IActionResult> RenwalMembership()
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var result = await _membershipService.CreateRenwalStripeCheckoutSessionAsync(User.GetUserId()!, domain);

            return result.IsSuccess
                ? Ok(new { redirectUrl = result.Value })
                : result.ToProblem();
        }

        [Authorize]
        [HttpGet("verifyRenwal")]
        public async Task<IActionResult> VerifyRenwalMembership([FromQuery] string session_id)
        {
            var result = await _membershipService.VerifyRenwalStripePaymentAsync(session_id);
            if (result.IsSuccess)
                return Ok(result.IsSuccess);

            return BadRequest(result.ToProblem());
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] UpdateMembershipRequest request, CancellationToken cancellationToken)
        {
            var result = await _membershipService.UpdateAsync(request, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _membershipService.DeleteAsync(id, cancellationToken);
            return result.IsSuccess ? Ok("✅ Membership deleted successfully.") : NotFound(result.Error);
        }



    }
}