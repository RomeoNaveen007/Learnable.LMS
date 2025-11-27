using FluentValidation;
using Learnable.Application.Features.Class.Commands.DeleteClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.DeleteRepository
{
    public class DeleteRepositoryCommandValidator : AbstractValidator<DeleteRepositoryCommand>
    {
        public DeleteRepositoryCommandValidator()
        {
            RuleFor(x => x.RepoId)
                .NotEmpty()
                .WithMessage("RepoId  is required.");
        }
    }
}
