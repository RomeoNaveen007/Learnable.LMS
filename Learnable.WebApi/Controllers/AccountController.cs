using Learnable.Application.Features.Users.Commands.RegisterUser;
using Learnable.Application.Features.Verifications.Commands.SendOtp;
using Learnable.Domain.Common.OTP;
using Learnable.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    public class AccountController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("send-otp")] // https://localhost:7180/Account/send-otp
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDto request)
        {
            var result = await _mediator.Send(new SendOtpCommand(request.Email));

            return Ok(new
            {
                message = "OTP sent successfully",
                success = result
            });
        }

        [HttpPost("register")] // https://localhost:7180/Account/register
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _mediator.Send(new RegisterUserCommand(request.User, request.Otp));

            return Ok(result);
        }
    }
}
