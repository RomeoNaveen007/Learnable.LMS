using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Features.Repository.Commands.UpdateRepository
{
    public class UpdateRepositoryDto
    {
        public Guid? ClassId { get; set; }
        public string RepoName { get; set; } = null!;
        public string? RepoDescription { get; set; }
        public string? RepoCertification { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
