using Learnable.Application.Features.Class.Commands.UpdateClass;
using Learnable.Application.Features.Exam.Commands.Create;
using Learnable.Application.Features.Exam.Commands.Delete;
using Learnable.Application.Features.Exam.Commands.Update;
using Learnable.Application.Features.Exam.Queries.GetByID;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{

    public class ExamController : BaseController
    {
        private readonly IMediator _mediator;

        public ExamController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // http://localhost:5071/api/Exam/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateExam(CreateExamCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExam(UpdateExamCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamById(Guid id)
        {
            var result = await _mediator.Send(new GetExamByIdQuery { ExamId = id });
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            var result = await _mediator.Send(new DeleteExamCommand(id));
            return Ok(result);
        }


    }
}

