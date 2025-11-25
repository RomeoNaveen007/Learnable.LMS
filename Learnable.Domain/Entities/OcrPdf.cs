using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

public partial class OcrPdf
{
    [Key]
    public Guid OcrPdfId { get; set; } // Auto-generated Unique ID

    public int ChunkId { get; set; } // Not auto-generated, not unique, not null

    public string Chunk { get; set; } = null!; // Not auto-generated, not unique, not null

    public Guid AssetId { get; set; } // Foreign key

    [ForeignKey("AssetId")]
    [InverseProperty("OcrPdfs")] // This links back to the Asset class
    public virtual Asset Asset { get; set; } = null!;
}