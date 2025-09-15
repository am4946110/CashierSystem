using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class Payment
{

    public Guid PaymentId { get; set; }

    public Guid SaleId { get; set; }

    public Guid PaymentTypeId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
