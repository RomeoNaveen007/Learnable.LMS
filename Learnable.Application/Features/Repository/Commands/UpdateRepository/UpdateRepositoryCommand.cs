using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.UpdateRepository
{
    public class UpdateRepositoryCommand : IRequest<Learnable.Application.Common.Dtos.RepositoryDtos?>
    {
        public Guid RepoId { get; set; }  // set in Controller
        public Guid ClassId { get; set; }  // set in Controller
        public string? RepoName { get; set; }
        public string? RepoDescription { get; set; }
        public string? RepoCertification { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
