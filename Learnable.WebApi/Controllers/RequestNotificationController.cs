using Learnable.Application.Features.RequestNotification.Commands.Approve;
using Learnable.Application.Features.RequestNotification.Commands.RejectRequest;
using Learnable.Application.Features.RequestNotification.Commands.Sent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{

    public class RequestNotificationController : BaseController
    {
        private readonly IMediator _mediator;

        public RequestNotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("class")]// http://localhost:5071/api/RequestNotification/class
        public async Task<IActionResult> CreateClassJoinRequest([FromBody] CreateClassJoinRequestCommand command)
        {
            var result = await _mediator.Send(command);

            if (result == null)
                return BadRequest(new { message = "Unable to create join request" });

            return Ok(result);
        }


        // http://localhost:5071/api/RequestNotification/approve

        [HttpPut("approve")]
        public async Task<IActionResult> Approve(ApproveRequestCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]

        [HttpPost("reject")]
        public async Task<IActionResult> Reject([FromBody] RejectRequestNotificationCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result) return NotFound(new { message = "Request not found" });
            return Ok(new { message = "Request rejected" });
        }

    }
}

