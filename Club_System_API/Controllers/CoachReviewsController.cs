using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class CoachReviewsController(ICoachReviewService coachRatingService) : ControllerBase
    {
        private readonly ICoachReviewService _coachReviewService= coachRatingService;

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpPost]
        public async Task<IActionResult> Add(ReviewCoachRequest request,
                 CancellationToken cancellationToken)
        {
            var result = await _coachReviewService.AddAsync(User.GetUserId(),request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [Authorize(Roles = nameof(DefaultRoles.Member))]
        [HttpPut("")]
        public async Task<IActionResult> Update(  ReviewCoachRequest request,
             CancellationToken cancellationToken)
        {
            var result = await _coachReviewService.UpdateAsync(User.GetUserId(), request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [HttpGet("Get-CoachReviews/{coachid}")]
        public async Task<IActionResult> Get([FromRoute] int coachid)
        {
            var result = await _coachReviewService.GetAsync(coachid);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

    }
}
