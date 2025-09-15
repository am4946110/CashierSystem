using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class PaymentType
{
    public Guid PaymentTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
