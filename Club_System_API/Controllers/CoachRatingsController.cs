using Club_System_API.Abstractions;
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
    [Authorize]
    public class CoachRatingsController(ICoachRatingService coachRatingService) : ControllerBase
    {
        private readonly ICoachRatingService _coachRatingService= coachRatingService;

        [HttpPost]
        public async Task<IActionResult> Add(RateCoachRequest request,
                 CancellationToken cancellationToken)
        {
            var result = await _coachRatingService.AddAsync(User.GetUserId(),request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [HttpPut("")]
        public async Task<IActionResult> Update(  RateCoachRequest request,
             CancellationToken cancellationToken)
        {
            var result = await _coachRatingService.UpdateAsync(User.GetUserId(), request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

    }
}
