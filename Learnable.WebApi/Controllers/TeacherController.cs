using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.Teacher.Commands.DeleteTeacher;
using Learnable.Application.Features.Teacher.Commands.UpdateTeacher;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    [Authorize]
    public class TeacherController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("update")]
        public async Task<ActionResult> UpdateTeacher(UpdateTeacherCommand command)
        {
            // Get logged-in UserId
            var userId = User.GetUserId();

            // Attach userId to command
            command.UserId = userId;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("Teacher not found");

            return Ok(new { message = "Teacher updated successfully" });
        }


        [HttpDelete("{profileId:guid}")]
        public async Task<ActionResult> DeleteTeacher(Guid profileId)
        {
            var result = await _mediator.Send(new DeleteTeacherCommand(profileId));

            if (!result)
                return NotFound("Teacher not found");

            return Ok(new { message = "Teacher deleted successfully" });
        }

    }
}
