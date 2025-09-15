using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class Payment
{
    [NotMapped]
    public string Id { get; set; }

    public Guid PaymentId { get; set; } = Guid.NewGuid();

    public Guid SaleId { get; set; }

    public Guid PaymentTypeId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? PaidAt { get; set; }

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
