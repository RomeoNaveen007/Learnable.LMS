using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Queries.GetByUnicName
{
    public class GetClassByUnicNameQueryValidator : AbstractValidator<GetClassByUnicNameQuery>
    {
        public GetClassByUnicNameQueryValidator()
        {
            RuleFor(x => x.UnicName)
                .NotEmpty().WithMessage("UnicName is required.")
                .MinimumLength(3).WithMessage("UnicName must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("UnicName cannot exceed 100 characters.");
        }
    }
}
