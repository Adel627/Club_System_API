using Club_System_API.Abstractions;
using Club_System_API.Abstractions.Consts;
using Club_System_API.Dtos.QA;
using Club_System_API.Extensions;
using Club_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Club_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof( DefaultRoles.Admin))]  
    public class QAsController(IQAService QAService) : ControllerBase
    {
        private readonly IQAService _QAService=QAService;



        [HttpPost]
        public async Task<IActionResult> Add( QARequest request,
       CancellationToken cancellationToken)
        {
            var result = await _QAService.AddAsync(User.GetUserId(),request, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();

        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            return Ok(await _QAService.GetAllAsync(cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _QAService.GetAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] QARequest request,
        CancellationToken cancellationToken)
        {
            var result = await _QAService.UpdateAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _QAService.DeleteAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }



    }
}
