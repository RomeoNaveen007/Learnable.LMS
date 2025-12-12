using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Asset.Queries.GetAssetByID_
{
    public class GetAssetWithOcrQueryValidator : AbstractValidator<GetAssetWithOcrQuery>
    {
        public GetAssetWithOcrQueryValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("AssetId is required.")
                .Must(id => id != Guid.Empty).WithMessage("AssetId cannot be an empty GUID.");
        }
    }
}
