using Club_System_API.Abstractions;
using Club_System_API.Dtos.CoachRating;
using Club_System_API.Dtos.ServiceReview;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceReviewsController(IServiceReviewService serviceReviewService ) : ControllerBase
    {
        private readonly IServiceReviewService _serviceReviewService = serviceReviewService;

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(ServiceReviewRequest request,
                CancellationToken cancellationToken)
        {
            var result = await _serviceReviewService.AddAsync(User.GetUserId(), request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> Update(ServiceReviewRequest request,
             CancellationToken cancellationToken)
        {
            var result = await _serviceReviewService.UpdateAsync(User.GetUserId(), request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [HttpGet("Get-ServiceReviews/{serviceid}")]
        public async Task<IActionResult> Get([FromRoute]int serviceid)
        {
            var result = await _serviceReviewService.GetAsync(serviceid);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
