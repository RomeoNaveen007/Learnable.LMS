using Learnable.Application.Common.Extensions;
using Learnable.Application.Features.RequestNotification.Commands.Approve;
using Learnable.Application.Features.RequestNotification.Commands.RejectRequest;
using Learnable.Application.Features.RequestNotification.Commands.Sent;
using Learnable.Application.Features.RequestNotification.Queries;
using Learnable.Application.Features.RequestNotification.Queries.Recevied;
using Learnable.Application.Features.RequestNotification.Queries.Sent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    [Authorize]
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

      

        [HttpPut("reject")]
        public async Task<IActionResult> Reject([FromBody] RejectRequestNotificationCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result) return NotFound(new { message = "Request not found" });
            return Ok(new { message = "Request rejected" });
        }


        [HttpGet("received")]
        public async Task<IActionResult> GetReceivedRequests()
        {
            var currentUserId = User.GetUserId();

            var result = await _mediator.Send(new GetReceivedRequestsQuery(currentUserId));

            return Ok(result);
        }

        ////// http://localhost:5071/api/RequestNotification/sent
        [HttpGet("sent")] // <--- UNCOMMENT THIS ATTRIBUTE
        public async Task<IActionResult> GetSentRequests() 
        {
            // 💡 ADD logic to get the current authenticated user ID
            var currentUserId = User.GetUserId();

            // 💡 PASS the UserId to the query handler
            var result = await _mediator.Send(new GetSentRequestsQuery(currentUserId));

            return Ok(result);
        }

    }
}

