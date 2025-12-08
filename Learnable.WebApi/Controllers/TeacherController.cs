using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.Teacher.Commands.DeleteTeacher;
using Learnable.Application.Features.Teacher.Commands.UpdateTeacher;
using Learnable.Application.Features.Teacher.Queries.GetAllTeachers;
using Learnable.Application.Features.Teacher.Queries.GetTeacherById;
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
            // Get logged-in UserId
            var userId = User.GetUserId();

            // Attach userId to command
            command.UserId = userId;

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound("Teacher not found");

            return Ok(new { message = "Teacher updated successfully" });
        }


        [HttpDelete("{UserId:guid}")]  // http://localhost:5071/api/teacher/
        public async Task<ActionResult> DeleteTeacher(Guid userId)
        {
            var result = await _mediator.Send(new DeleteTeacherByUserIdCommand(userId));

            if (!result)
                return NotFound("Teacher not found");

            return Ok(new { message = "Teacher deleted successfully" });
        }

        [HttpGet("all")]  // http://localhost:5071/api/teacher/all
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTeachersQuery());
            return Ok(result);
        }

        [HttpGet("{profileId}")]  // http://localhost:5071/api/teacher/
        public async Task<IActionResult> GetById(Guid profileId)
        {
            // Get logged-in UserId
            var userId = User.GetUserId();

            var result = await _mediator.Send(new GetTeacherByIdQuery
            {
                ProfileId = profileId,
                LoggedInUserId = userId
            });

            if (result == null)
                return NotFound("Teacher not found.");

            return Ok(result);
        }

    }
}
