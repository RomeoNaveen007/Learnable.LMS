using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    public class AssetDto
    {
        public Guid AssetId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public Guid RepoId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string Url { get; set; }
        public List<OcrPdfDto> OcrPdfs { get; set; }
    }
}
