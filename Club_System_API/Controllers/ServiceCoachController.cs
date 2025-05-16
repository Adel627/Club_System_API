using Club_System_API.Abstractions;
using Club_System_API.Dtos.ServiceCoach;
using Club_System_API.Models;
using Club_System_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCoachController(IServiceCoachService serviceCoachService) : ControllerBase
    {
        private readonly IServiceCoachService _serviceCoachService = serviceCoachService;

        [HttpPost("Assign-Coach-To-Service")]
        public async Task<IActionResult>Add(ServiceCoachRequest request,
            CancellationToken cancellationToken)
        {
         var result= await  _serviceCoachService.AddCoachToServiceAsync(request, cancellationToken);
            return result.IsSuccess ? Created(): result.ToProblem();
        }
        [HttpDelete("Remove-Coach-From-Service")]
        public async Task<IActionResult> Remove(ServiceCoachRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _serviceCoachService.RemoveCoachFromServiceAsync(request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }


    }
}
