using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("Prompt")]
public partial class Prompt
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string? PromptCode { get; set; }

    public string? PromptText { get; set; }
}
