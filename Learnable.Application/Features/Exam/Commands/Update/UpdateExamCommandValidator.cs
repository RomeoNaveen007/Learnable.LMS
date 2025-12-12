using FluentValidation;
using Learnable.Application.Features.Exam.Commands.Update;

namespace Learnable.Application.Features.Exam.Commands.Update.Validator
{
    public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
    {
        public UpdateExamCommandValidator()
        {
            RuleFor(x => x.Exam)
                .NotNull().WithMessage("Exam data is required.");

            RuleFor(x => x.Exam)
                .SetValidator(new UpdateExamDtoValidator()!);
        }
    }

    public class UpdateExamDtoValidator : AbstractValidator<UpdateExamDto>
    {
        public UpdateExamDtoValidator()
        {
            RuleFor(x => x.ExamId)
                .NotEmpty().WithMessage("ExamId is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.StartDatetime)
                .NotNull().WithMessage("StartDatetime is required.");

            RuleFor(x => x.EndDatetime)
                .NotNull().WithMessage("EndDatetime is required.")
                .GreaterThan(x => x.StartDatetime)
                .WithMessage("EndDatetime must be after StartDatetime.");

            RuleFor(x => x.Duration)
                .NotNull().WithMessage("Duration is required.")
                .GreaterThan(0).WithMessage("Duration must be greater than 0.");

            RuleForEach(x => x.Questions)
                .SetValidator(new UpdateExamQuestionDtoValidator()!);

            RuleFor(x => x.Questions)
                .NotNull().WithMessage("Questions list cannot be null.")
                .Must(q => q.Count > 0).WithMessage("At least one question is required.");
        }
    }

    public class UpdateExamQuestionDtoValidator : AbstractValidator<UpdateExamQuestionDto>
    {
        public UpdateExamQuestionDtoValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question text is required.")
                .MaximumLength(500).WithMessage("Question cannot exceed 500 characters.");

            RuleFor(x => x.Answers)
                .NotNull().WithMessage("Answers list cannot be null.")
                .Must(a => a.Count >= 2).WithMessage("Each question must have at least 2 answers.");

            RuleForEach(x => x.Answers)
                .NotEmpty().WithMessage("Answer text cannot be empty.")
                .MaximumLength(250).WithMessage("Answer text cannot exceed 250 characters.");

            RuleFor(x => x.CorrectAnswerIndex)
                .GreaterThanOrEqualTo(0).WithMessage("CorrectAnswerIndex must be 0 or higher.")
                .Must((model, index) =>
                    model.Answers != null && index < model.Answers.Count)
                .WithMessage("CorrectAnswerIndex must point to a valid answer option.");
        }
    }
}
