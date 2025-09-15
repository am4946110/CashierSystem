using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class VwCustomerSummary
{
    public Guid CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public int? TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }

    public DateTime? FirstPurchase { get; set; }

    public DateTime? LastPurchase { get; set; }
}
