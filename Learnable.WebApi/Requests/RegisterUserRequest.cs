using Learnable.Application.Features.Users.Commands.RegisterUser;

namespace Learnable.WebApi.Requests
{
    public class RegisterUserRequest
    {
        public RegisterUserDto User { get; set; } = null!;
        public string Otp { get; set; } = null!;
    }
}
