using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class RepositoryDtos
    {
        public Guid RepoId { get; set; }
        public Guid? ClassId { get; set; }
        public string RepoName { get; set; } = null!;
        public string? RepoDescription { get; set; }
        public string? RepoCertification { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
    }
}
