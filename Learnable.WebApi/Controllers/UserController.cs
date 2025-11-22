using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.User.Commands.DeleteUser;
using Learnable.Application.Features.User.Commands.UpdateUser;
using Learnable.Application.Features.User.Queries.GetAllUsers;
using Learnable.Application.Features.User.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Learnable.WebApi.Controllers
{
    [Authorize]
    public class UserController(IMediator mediator) :BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("update")] // http://localhost:5071/api/User/update
        public async Task<ActionResult> UpdateUser(UpdateUserCommand command)
        {
            var userId = User.GetUserId();
            command.UserId = userId ;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("User not found");

            return Ok(new
            {
                message = "User Updated sucessfuly",
            });
        }

        [HttpGet("all")]  // http://localhost:5071/api/User/all
        public async Task<ActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")] // http://localhost:5071/api/User/
        public async Task<ActionResult> GetUserById(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]  // http://localhost:5071/api/User/
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var userId = User.GetUserId();

            var result = await _mediator.Send(new DeleteUserCommand (id));

            if (!result)
                return NotFound("User not found");

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
