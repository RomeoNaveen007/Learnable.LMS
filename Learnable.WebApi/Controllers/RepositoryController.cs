using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Repository.Commands.AddRepository;
using Learnable.Application.Features.Repository.Commands.DeleteRepository;
using Learnable.Application.Features.Repository.Commands.UpdateRepository;
using Learnable.Application.Features.Repository.Queries.GetAll;
using Learnable.Application.Features.Repository.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    public class RepositoryController : BaseController
    {
        private readonly IMediator _mediator;

        public RepositoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Repository
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RepositoryDtos>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllRepositoryQuery());
            return Ok(result);
        }

        // GET: api/Repository/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RepositoryDtos>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetRepositoryByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/Repository
        [HttpPost]
        public async Task<ActionResult<RepositoryDtos>> Create([FromBody] CreateRepositoryCommand request)
        {
            if (request == null)
                return BadRequest("Repository data is required");

            var result = await _mediator.Send(request);

            // *** FIXED ***
            return Ok(result);
        }

        // PUT: api/Repository/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRepositoryCommand command)
        {
            if (command == null || id != command.RepoId)
                return BadRequest("Invalid repository data or ID mismatch");

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE: api/Repository/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteRepositoryCommand(id);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
