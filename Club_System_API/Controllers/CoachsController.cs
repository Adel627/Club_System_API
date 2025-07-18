using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Coaches;
using Club_System_API.Extensions;
using Club_System_API.Models;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachsController : ControllerBase
    {

        private readonly ICoachService _coachService;
        public CoachsController(ICoachService coachService)
        {
            _coachService = coachService;

        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost]
        public async Task<IActionResult> Add( [FromForm]CoachRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _coachService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();

        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost("Achievment/{coachid}")]
        public async Task<IActionResult> AddAchievment([FromRoute] int coachid, AchievmentRequest request,
       CancellationToken cancellationToken)
        {
            var result = await _coachService.AddAchievmentAsync(coachid,request, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();

        }


        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> GetAll( CancellationToken cancellationToken)
        {
            bool isadmin = false;
            if(User.IsInRole(nameof( DefaultRoles.Admin)))
                isadmin = true;
            return Ok(await _coachService.GetAllAsync(isadmin, cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _coachService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CoachRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _coachService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var result = await _coachService.ToggleStatus(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _coachService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

    }
}
