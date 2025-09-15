using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class VwPayment
{
    public Guid PaymentId { get; set; }

    public DateTime? PaidAt { get; set; }

    public Guid SaleId { get; set; }

    public string? CustomerName { get; set; }

    public string PaymentType { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal? NetAmount { get; set; }
}
