using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Application.Common.Dtos
{
    internal class OcrPdfDto
    {
        public Guid OcrPdfId { get; set; }
        public int ChunkId { get; set; }
        public string Chunk { get; set; } = null!;
    }
}
