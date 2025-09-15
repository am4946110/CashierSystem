using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class PaymentType
{
    [NotMapped]
    public string Id { get; set; }
    public Guid PaymentTypeId { get; set; } = Guid.NewGuid();

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
