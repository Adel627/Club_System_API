using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Membership;
using Club_System_API.Extensions;
using Club_System_API.Helper;
using Club_System_API.Services;

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

       // [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MembershipRequest request, CancellationToken cancellationToken)
        {
            var result = await _membershipService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _membershipService.GetAllAsync(cancellationToken);
            return Ok(result);
        }

        /// Assign a membership directly to the authenticated user (no payment).
        [Authorize]
        [HttpPost("assign/{membershipId}")]
        public async Task<IActionResult> AssignMembership([FromRoute] int membershipId)
        {
            var result = await _membershipService.AssignToUserAsync(User.GetUserId(), membershipId);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        // Create Stripe Checkout Session to pay for selected membership.
        [Authorize]
        [HttpPost("select")]
        public async Task<IActionResult> SelectMembership([FromBody] MembershipRequest request)
        {
            var domain = $"{Request.Scheme}://{Request.Host}";
            var result = await _membershipService.CreateStripeCheckoutSessionAsync(User.GetUserId()!, request.MembershipId, domain);

            return result.IsSuccess
                ? Ok(new { redirectUrl = result.Value })
                : result.ToProblem();
        }

        // Stripe webhook to confirm payment and assign membership.
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, _stripeSettings.WebhookSecret);
            }
            catch (StripeException ex)
            {
                return BadRequest(new { message = $"Stripe webhook error: {ex.Message}" });
            }

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                if (session != null)
                {
                    await _membershipService.AssignMembershipAfterPaymentAsync(session.Id);
                }
            }


            return Ok();
        }
    }
}
