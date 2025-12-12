using FluentValidation;
using Learnable.Application.Features.AiServises.Queries.Question.GetQuestions;

public class GetQuestionQueryValidator : AbstractValidator<GetQuestionQuery>
{
    public GetQuestionQueryValidator()
    {
        RuleFor(x => x.Asset_Id)
            .NotNull().WithMessage("Asset_Id list is required.")
            .Must(ids => ids!.Count > 0).WithMessage("At least one Asset_Id must be provided.");

        RuleForEach(x => x.Asset_Id)
            .NotEmpty().WithMessage("Asset_Id cannot contain empty GUIDs.");

    }
}
