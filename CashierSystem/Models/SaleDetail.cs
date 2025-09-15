using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class SaleDetail
{
    public Guid SaleDetailId { get; set; }

    public Guid SaleId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
