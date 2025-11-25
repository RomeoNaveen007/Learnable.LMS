using Learnable.Application.Features.Class.Commands.AddClass;
using Learnable.Application.Features.Class.Commands.DeleteClass;
using Learnable.Application.Features.Class.Commands.UpdateClass;
using Learnable.Application.Features.Class.Queries;
using Learnable.Application.Features.Class.Queries.GetAll;
using Learnable.Application.Features.Class.Queries.GetById;
using Learnable.Application.Features.Class.Queries.GetByUnicName;
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
        [HttpPost("create")] // http://localhost:5071/api/Class/create
        public async Task<IActionResult> CreateClass([FromBody] CreateClassCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
            {
                return BadRequest(new { message = "Unable to create class" });
            }

            return Ok(result); ;
        }


        /// <summary>
        /// Update an existing Class
        /// PUT: /api/Class/update
        /// </summary>

        [HttpPut("{id}")]/// POST:http://localhost:5071 /api/Class/10C7AD0F-AE64-4DB4-9B98-2788A35BDF8D
        public async Task<IActionResult> UpdateClass(Guid id, [FromBody] UpdateClassCommand command)
        {
            command.ClassId = id;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound(new { message = "Class not found" });

            return Ok(result);
        }

        /// <summary>
        /// Get class by id
        /// GET: /api/Class/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        /// GET: http://localhost:5071/api/Class/10C7AD0F-AE64-4DB4-9B98-2788A35BDF8D
        public async Task<ActionResult> GetClassById(Guid id)
        {
            var result = await _mediator.Send(new GetClassByIdQuery { ClassId = id });

            if (result == null)
                return NotFound(new { message = "Class not found" });

            return Ok(result);
        }

        /// <summary>
        /// Get class by id
        /// GET: /api/Class/{id}
        /// </summary>"{unicname}"
        [HttpGet("{unicname}")]
        /// GET: http://localhost:5071/api/Class/unicname
        public async Task<ActionResult> GetClassByUnicname(string unicname)
        {
            var result = await _mediator.Send(new GetClassByUnicNameQuery { UnicName = unicname });

            if (result == null)
                return NotFound(new { message = "Class not found" });

            return Ok(result);
        }



        /// <summary>
        /// Get all classes
        /// GET: /api/Class/all
        /// </summary>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllClasses()
        {
            var result = await _mediator.Send(new GetAllClassesQuery());
            return Ok(result);
        }


        /// <summary>
        /// Delete class by id
        /// DELETE: /api/Class/{id}
        /// </summary>
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
