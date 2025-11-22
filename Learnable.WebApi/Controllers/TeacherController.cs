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

        [HttpPut("update")]  // http://localhost:5071/api/teacher/update
        public async Task<ActionResult> UpdateTeacher(UpdateTeacherCommand command)
        {
            var userId = User.GetUserId();

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("Teacher not found");

            return Ok(new
            {
                message = "Teacher updated successfully"
            });
        }

        [HttpDelete("{profileId:guid}")]   // http://localhost:5071/api/teacher/id
        public async Task<ActionResult> DeleteTeacher(Guid profileId)
        {
            var userId = User.GetUserId();

            var result = await _mediator.Send(new DeleteTeacherCommand(userId));

            if (!result)
                return NotFound("Teacher not found or you are not authorized to delete this teacher");

            return Ok(new { message = "Teacher deleted successfully" });
        }
    }
}
