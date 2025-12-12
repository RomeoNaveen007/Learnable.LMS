using FluentValidation;

namespace Learnable.Application.Features.Marks.Commands.CreateMark
{
    public class AddMarkCommandValidator : AbstractValidator<AddMarkCommand>
    {
        public AddMarkCommandValidator()
        {
            RuleFor(x => x.ExamId)
                .NotEmpty().WithMessage("ExamId is required.");

            RuleFor(x => x.ClassId)
                .NotEmpty().WithMessage("ClassId is required.");
        }
    }
}
