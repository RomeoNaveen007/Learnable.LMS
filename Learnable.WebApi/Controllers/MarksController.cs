using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Marks.Commands.CreateMark;
using Learnable.Application.Features.Marks.Commands.DeleteMark;
using Learnable.Application.Features.Marks.Commands.UpdateMark;
using Learnable.Application.Features.Marks.Queries.GetMark;
using Learnable.Application.Features.Marks.Queries.GetMarksByExam;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MarksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1️⃣ Get Mark by ExamId + StudentId
        [HttpGet("{examId:guid}/{studentId:guid}")]
        public async Task<ActionResult<MarksDto?>> GetMark(Guid examId, Guid studentId)
        {
            var query = new GetMarkQuery(examId, studentId);
            var mark = await _mediator.Send(query);

            if (mark == null) return NotFound();
            return Ok(mark);
        }

        // 2️⃣ Get Marks by ExamId
        [HttpGet("exam/{examId:guid}")]
        public async Task<ActionResult<List<MarksDto>>> GetMarksByExam(Guid examId)
        {
            var query = new GetMarksByExamIdQuery(examId);
            var marks = await _mediator.Send(query);
            return Ok(marks);
        }

        // 3️⃣ Update Mark
        [HttpPut]
        public async Task<ActionResult<MarksDto?>> UpdateMark([FromBody] MarksDto markDto)
        {
            var command = new UpdateMarkCommand(markDto);
            var updatedMark = await _mediator.Send(command);

            if (updatedMark == null) return NotFound();
            return Ok(updatedMark);
        }

        // 4️⃣ Delete Mark
        [HttpDelete("{examId:guid}/{studentId:guid}")]
        public async Task<ActionResult> DeleteMark(Guid examId, Guid studentId)
        {
            var command = new DeleteMarkCommand(examId, studentId);
            var result = await _mediator.Send(command);

            if (!result) return NotFound();
            return NoContent();
        }

        // POST: api/marks/create-default
        [HttpPost("create-default")]
        public async Task<IActionResult> CreateDefaultMarks([FromBody] AddMarkCommand command)
        {
            if (command == null)
                return BadRequest("Invalid request");

            var result = await _mediator.Send(command);

            return Ok(new
            {
                Message = "Default marks added successfully",
                ExamId = command.ExamId,
                ClassId = command.ClassId,
                StudentIds = result
            });
        }
    }
}
