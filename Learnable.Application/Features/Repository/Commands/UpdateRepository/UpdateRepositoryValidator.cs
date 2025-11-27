using FluentValidation;
using Learnable.Application.Features.Class.Commands.UpdateClass;
using Learnable.Application.Features.Repository.Commands.UpdateRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.UpdateRepository
{
    public class UpdateRepositoryValidator : AbstractValidator<UpdateRepositoryCommand>
    {
        public UpdateRepositoryValidator()
        {
            RuleFor(x => x.RepoId)
                .NotEmpty()
                .WithMessage("RepoId  is required");

            RuleFor(x => x.RepoName)
                .MaximumLength(100)
                .When(x => !string.IsNullOrEmpty(x.RepoName));

            RuleFor(x => x.RepoDescription)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.RepoDescription));

            RuleFor(x => x.RepoCertification)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.RepoCertification));

            RuleFor(x => x.Status)
                .MaximumLength(20)
                .When(x => !string.IsNullOrEmpty(x.Status));

            RuleFor(x => x.CreatedAt)
               .LessThanOrEqualTo(DateTime.UtcNow)
               .When(x => x.CreatedAt != null)
               .WithMessage("CreatedAt cannot be in the future");
        }
    }
}


