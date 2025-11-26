using Learnable.Application.Common.Dtos;
using Learnable.Application.Features.Account.Commands.RegisterTeacher;
using Learnable.Application.Features.Account.Queries.LoginUser;
using Learnable.Application.Features.Users.Commands.RegisterUser;
using Learnable.Application.Features.Users.Queries.LoginUser;
using Learnable.Application.Features.Verifications.Commands.SendOtp;
using Learnable.Domain.Common.OTP;
using Learnable.WebApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    public class AccountController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("send-otp")] //  http://localhost:5071/api/Account/send-otp
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new SendOtpCommand(request.Email));

            if (!result)
                return StatusCode(500, new { message = "Failed to send OTP check the Email", success = false });

            return Ok(new
            {
                message = "OTP sent successfully",
                success = result    
            });
        }

        [HttpPost("register")] //  http://localhost:5071/api/Account/register
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await _mediator.Send(new RegisterUserCommand(request.User, request.Otp));

            return Ok(result);
        }

        [HttpPost("login")]   //  http://localhost:5071/api/Account/login
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _mediator.Send(
                new LoginQuery(dto.Email, dto.Password)    
            );

            return Ok(result); // returns LoginResponseDto
        }

        [HttpPost("register-teacher")]  //  http://localhost:5071/api/Account/register-teacher
        public async Task<ActionResult<TeacherUserDto>> RegisterTeacher([FromBody] RegisterTeacherCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
