
using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Membership;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipsController(IMembershipService membershipService) : ControllerBase
    {
        private readonly IMembershipService _membershipService = membershipService;

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost]
        public async Task<IActionResult> Add(MembershipRequest request, CancellationToken cancellationToken)
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

        [Authorize]
        [HttpPost("assign/{membershipId}")]
        public async Task<IActionResult> AssignMembership([FromRoute] int membershipId)
        {
            var result = await _membershipService.AssignToUserAsync(User.GetUserId(), membershipId);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
