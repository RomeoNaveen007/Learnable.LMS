using Learnable.Application.Features.Class.Commands.AddClass;
using Learnable.Application.Features.Class.Commands.DeleteClass;
using Learnable.Application.Features.Class.Commands.UpdateClass;
using Learnable.Application.Features.Class.Queries.GetAll;
using Learnable.Application.Features.Class.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClassController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClassController(IMediator mediator)
        {
            _mediator = mediator;
        }

     
        [HttpPost("create")]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest(new { message = "Unable to create class" });
            }

            return Ok(new
            {
                message = "Class created successfully"
            });
        }


        [HttpPut("{id}")]/// POST:http://localhost:5071 /api/Class/10C7AD0F-AE64-4DB4-9B98-2788A35BDF8D
        public async Task<IActionResult> UpdateClass(Guid id, [FromBody] UpdateClassCommand command)
        {
            command.ClassId = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new { message = "Class not found" });

            return Ok(new
            {
                message = "Class updated successfully",
                data = result
            });
        }



        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetClassById(Guid id)
        {
            var result = await _mediator.Send(new GetClassByIdQuery(id));

            if (result == null)
                return NotFound(new { message = "Class not found" });

            return Ok(result);
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllClasses()
        {
            var result = await _mediator.Send(new GetAllClassesQuery());
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            var result = await _mediator.Send(new DeleteClassCommand(id));

            if (!result)
                return NotFound(new { message = "Class not found" });

            return Ok(new { message = "Class deleted successfully" });
        }
    }

}
