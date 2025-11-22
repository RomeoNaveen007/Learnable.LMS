using Learnable.Application.Features.Class.Commands.AddClass;
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

        /// <summary>
        /// Create a new Class
        /// POST: /api/Class/create
        /// </summary>
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
    

    /// <summary>
    /// Update an existing Class
    /// PUT: /api/Class/update
    /// </summary>
    //[HttpPut("update")]  /// POST:http://localhost:5071 /api/Class/update
    //public async Task<ActionResult> UpdateClass([FromBody] UpdateClassCommand command)
    //{
    //    var result = await _mediator.Send(command);

    //    if (result == null)
    //        return NotFound(new { message = "Class not found" });

    //    return Ok(new
    //    {
    //        message = "Class updated successfully",
    //        data = result
    //    });
    //}

    /// <summary>
    /// Get all classes
    /// GET: /api/Class/all
    /// </summary>
    //[HttpGet("all")] /// POST:http://localhost:5071 /api/Class/all
    //public async Task<ActionResult> GetAllClasses()
    //{
    //    var result = await _mediator.Send(new GetAllClassesQuery());
    //    return Ok(result);
    //}

    /// <summary>
    /// Get class by id
    /// GET: /api/Class/{id}
    /// </summary>
    //[HttpGet("{id:guid}")] 
    //public async Task<ActionResult> GetClassById(Guid id)
    //{
    //    var result = await _mediator.Send(new GetClassByIdQuery(id));

    //    if (result == null)
    //        return NotFound(new { message = "Class not found" });

    //    return Ok(result);
    //}

    /// <summary>
    /// Delete class by id
    /// DELETE: /api/Class/{id}
    /// </summary>
    //[HttpDelete("{id:guid}")]
    //public async Task<ActionResult> DeleteClass(Guid id)
    //{
    //    var result = await _mediator.Send(new DeleteClassCommand(id));

    //    if (!result)
    //        return NotFound(new { message = "Class not found" });

    //    return Ok(new { message = "Class deleted successfully" });
    //}
}
    
}
