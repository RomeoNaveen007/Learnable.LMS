using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learnable.Domain.Errors
{
    [Table("ApiException")]
    public class ApiException
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public int StatusCode { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = null!;

        [StringLength(2000)]
        public string? Details { get; set; }


        // Custom constructor for creating exception logs
        public ApiException(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }

}
