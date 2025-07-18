using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Appointment;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService = appointmentService;


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost()]
        public async Task<IActionResult> Add(AppointmentRequest request, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) 
            {
               return BadRequest(ModelState);
            }
            var result = await _appointmentService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? Created() : result.ToProblem();   
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _appointmentService.GetAllAsync(cancellationToken));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpGet("appointmsOfService/{serviceid}")]
        public async Task<IActionResult> GetAll([FromRoute] int serviceid, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAllOfServiceAsync(serviceid, cancellationToken);

            return Ok(result);
        }

        [HttpGet("appointmsOfcoach/{coachid}")]
        public async Task<IActionResult> GetAllOfCoachAsync([FromRoute] int coachid, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.GetAllOfCoachAsync(coachid, cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AppointmentUpdateRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }



    }
}
