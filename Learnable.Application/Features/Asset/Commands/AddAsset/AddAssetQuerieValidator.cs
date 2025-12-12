using FluentValidation;

namespace Learnable.Application.Features.Asset.Commands.AddAsset
{
    public class AddAssetQuerieValidator : AbstractValidator<AddAssetQuerie>
    {
        public AddAssetQuerieValidator()
        {
            // Required fields
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required.")
                .MaximumLength(50).WithMessage("Type cannot exceed 50 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url is required.")
                .MaximumLength(500).WithMessage("Url cannot exceed 500 characters.")
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Url must be a valid absolute URL.");

            RuleFor(x => x.RepoId)
                .NotEmpty().WithMessage("RepoId is required.");

        }

        // Nested validator for OCR chunks
        public class OcrPdfDtoValidator : AbstractValidator<AddAssetQuerie.OcrPdfDto>
        {
            public OcrPdfDtoValidator()
            {
                RuleFor(x => x.ChunkId)
                    .GreaterThan(0).WithMessage("ChunkId must be greater than 0.");

                RuleFor(x => x.Chunk)
                    .NotEmpty().WithMessage("Chunk text cannot be empty.");
            }
        }
    }
}
