using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.AddRepository
{
    public class CreateRepositoryValidator: AbstractValidator<CreateRepositoryDto>
    {
        public CreateRepositoryValidator() 
        {
            RuleFor(x => x.RepoName)
               .NotEmpty().WithMessage("Repository name is required")
               .MaximumLength(100);

            RuleFor(x => x.RepoDescription)
                .NotEmpty().WithMessage("RepoDescription name is required")
                .MaximumLength(50);

            RuleFor(x => x.RepoCertification)
                .MaximumLength(200);

            RuleFor(x => x.Status)
                .MaximumLength(20);
        }
    }
}
