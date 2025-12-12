using FluentValidation;
using Learnable.Application.Features.Account.Commands.RegisterTeacher;

namespace Learnable.Application.Features.Account.Commands.RegisterTeacher
{
    public class RegisterTeacherDtoValidator : AbstractValidator<RegisterTeacherDto>
    {
        public RegisterTeacherDtoValidator()
        {
            // Required fields
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            // Optional user profile fields
            RuleFor(x => x.FullName)
                .MaximumLength(150).WithMessage("Full Name cannot exceed 150 characters.");

            RuleFor(x => x.DisplayName)
                .MaximumLength(100).WithMessage("Display Name cannot exceed 100 characters.");

            // Teacher fields
            RuleFor(x => x.DateOfBirth)
                .Must(d => d == null || d <= DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of birth cannot be in the future.");

            RuleFor(x => x.ContactPhone)
                .MaximumLength(20)
                .WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(x => x.Bio)
                .MaximumLength(500)
                .WithMessage("Bio cannot exceed 500 characters.");

            RuleFor(x => x.AvatarUrl)
                .MaximumLength(300)
                .WithMessage("Avatar URL cannot exceed 300 characters.");
        }
    }
}
