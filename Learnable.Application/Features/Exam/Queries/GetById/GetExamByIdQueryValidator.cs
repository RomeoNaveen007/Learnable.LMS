using FluentValidation;

namespace Learnable.Application.Features.Exam.Queries.GetByID
{
    public class GetExamByIdQueryValidator : AbstractValidator<GetExamByIdQuery>
    {
        public GetExamByIdQueryValidator()
        {
            RuleFor(x => x.ExamId)
                .NotEmpty().WithMessage("ExamId is required.");
        }
    }
}
