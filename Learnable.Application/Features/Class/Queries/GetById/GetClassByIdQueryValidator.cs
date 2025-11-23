using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Class.Queries.GetById
{
    public class GetClassByIdQueryValidator : AbstractValidator<GetClassByIdQuery>
    {
        public GetClassByIdQueryValidator()
        {
            RuleFor(x => x.ClassId)
                .NotEmpty()
                .WithMessage("ClassId is required.");
        }
    }
}
