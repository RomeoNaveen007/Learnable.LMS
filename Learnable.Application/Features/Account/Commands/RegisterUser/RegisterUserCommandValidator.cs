using FluentValidation;

namespace Learnable.Application.Features.Account.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            // Ensure DTO exists
            RuleFor(x => x.Dto)
                .NotNull()
                .WithMessage("User data is required.");

            // ---- Email ----
            RuleFor(x => x.Dto.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            // ---- Username ----
            RuleFor(x => x.Dto.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

            // ---- Password ----
            RuleFor(x => x.Dto.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            // ---- Display Name (Optional) ----
            RuleFor(x => x.Dto.DisplayName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.DisplayName));

            // ---- Full Name (Optional) ----
            RuleFor(x => x.Dto.FullName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Dto.FullName));

            // ---- OTP ----
            RuleFor(x => x.OtpCode)
                .NotEmpty().WithMessage("OTP is required.")
                .Matches(@"^\d{4}$").WithMessage("OTP must be a 4-digit number.");
        }
    }
}
