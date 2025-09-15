using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class StockTransaction
{

    public Guid TransactionId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public string? TransactionType { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Reference { get; set; }

    public virtual Product Product { get; set; } = null!;
}
