using Club_System_API.Abstractions;
using Club_System_API.Dtos.ClubReview;
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
    public class ClubReviewsController(IClubReviewService clubReviewService) : ControllerBase
    {
        private readonly IClubReviewService _clubReviewService=clubReviewService;  
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(ClubReviewRequest request,
                CancellationToken cancellationToken)
        {
            var result = await _clubReviewService.AddAsync(User.GetUserId(), request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> Update(ClubReviewRequest request,
             CancellationToken cancellationToken)
        {
            var result = await _clubReviewService.UpdateAsync(User.GetUserId(), request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _clubReviewService.GetAllAsync();
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
    }
}
