using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.Service;
using Club_System_API.Services.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _serviceService;
        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;

        }

        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPost]
        public async Task<IActionResult> Add( [FromForm]ServiceRequest request,
       CancellationToken cancellationToken)
        {
            var result = await _serviceService.AddAsync(request, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();

        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _serviceService.GetAllAsync(cancellationToken));
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _serviceService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] ServiceRequest request,
        CancellationToken cancellationToken)
        {
            var result = await _serviceService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [Authorize(Roles = nameof(DefaultRoles.Admin))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _serviceService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


    }
}
