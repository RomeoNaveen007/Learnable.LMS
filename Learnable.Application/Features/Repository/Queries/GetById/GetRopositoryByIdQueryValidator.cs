using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Queries.GetById
{
    public class GetRepositoryByIdQueryValidator : AbstractValidator<GetRepositoryByIdQuery>
    {
        public GetRepositoryByIdQueryValidator()
        {
            RuleFor(x => x.RepoId)
                .NotEmpty()
                .WithMessage("RepoId is required.");
        }
    }
}
