using Club_System_API.Abstractions;
using Club_System_API.Dtos.Appointment;
using Club_System_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService = appointmentService;

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

    }
}
