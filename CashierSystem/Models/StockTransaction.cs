using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class StockTransaction
{
    [NotMapped]
    public string Id { get; set; }

    public Guid TransactionId { get; set; } = Guid.NewGuid();

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public string? TransactionType { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Reference { get; set; }

    public virtual Product Product { get; set; } = null!;
}
