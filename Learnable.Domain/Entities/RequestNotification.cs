using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Learnable.Domain.Entities;

[Table("RequestNotification")]
public partial class RequestNotification
{
    [Key]
    public Guid NotificationId { get; set; }

    public Guid? SenderId { get; set; }

    public Guid? ReceiverId { get; set; }

    [StringLength(50)]
    public string? NotificationStatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ReceiverId")]
    [InverseProperty("RequestNotificationReceivers")]
    public virtual User? Receiver { get; set; }

    [ForeignKey("SenderId")]
    [InverseProperty("RequestNotificationSenders")]
    public virtual User? Sender { get; set; }
}
