using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Domain.Entities
{
    public class Ocr_Skan
    {
        public Guid Id { get; set; }

        // Foreign Key
        public Guid AssetId { get; set; }

        public string? OcrText { get; set; }

        // Navigation
        public virtual Asset Asset { get; set; } = null!;
    }
}
