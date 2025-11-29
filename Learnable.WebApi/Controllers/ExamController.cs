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

        // http://localhost:5071/api/Exam/
        [HttpPut]
        public async Task<IActionResult> UpdateExam(UpdateExamCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // http://localhost:5071/api/Exam/id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamById(Guid id)
        {
            var result = await _mediator.Send(new GetExamByIdQuery { ExamId = id });
            return Ok(result);
        }

        // http://localhost:5071/api/Exam/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(Guid id)
        {
            var result = await _mediator.Send(new DeleteExamCommand(id));
            return Ok(result);
        }


    }
}


//Data Check pannu  
//Create Exam

//{
//    "exam": {
//        "repoId": "7f7d4b40-5d0f-4c8f-8f5c-9ae3ce129a21",
//    "title": "Mid Term Science Exam",
//    "description": "60 marks",
//    "startDatetime": "2025-01-02T10:00:00",
//    "endDatetime": "2025-01-02T11:00:00",
//    "duration": 60,
//    "questions": [
//      {
//            "question": "What is the speed of light?",
//        "answers": [
//          "3x10^8 m/s",
//          "3x10^6 m/s",
//          "3x10^5 m/s",
//          "3x10^2 m/s"
//        ],
//        "correctAnswerIndex": 0
//      }
//    ]
//  }
//}
