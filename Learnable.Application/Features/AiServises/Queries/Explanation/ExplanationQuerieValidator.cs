using FluentValidation;
using Learnable.Application.Features.AiServises.Queries.Explanation;

public class ExplanationQuerieValidator : AbstractValidator<ExplanationQuerie>
{
    public ExplanationQuerieValidator()
    {
        RuleFor(x => x.Input)
            .NotNull()
            .WithMessage("Input is required.");
    }
}
