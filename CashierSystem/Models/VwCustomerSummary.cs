using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class VwCustomerSummary
{
    [NotMapped]
    public string Id { get; set; }
    public Guid CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public int? TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }

    public DateTime? FirstPurchase { get; set; }

    public DateTime? LastPurchase { get; set; }
}
