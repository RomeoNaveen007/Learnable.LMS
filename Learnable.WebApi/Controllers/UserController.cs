using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.User.Commands.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Learnable.WebApi.Controllers
{
    public class UserController(IMediator mediator) :BaseController
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpPut("update")] // http://localhost:5071/api/User/update
        public async Task<ActionResult> UpdateUser(UpdateUserCommand command)
        {
            var userId = User.GetUserId();
            command.UserId = Guid.Parse(userId);

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("User not found");

            return Ok(new
            {
                message = "User Updated sucessfuly",
            });
        }
    }
}
