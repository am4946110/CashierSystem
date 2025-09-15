using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class SaleDetail
{
    [NotMapped]
    public string Id { get; set; }
    public Guid SaleDetailId { get; set; } = Guid.NewGuid();

    public Guid SaleId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Total
    {
        get => Quantity * UnitPrice;
        private set { } 
    }

    public virtual Product Product { get; set; } = null!;

    public virtual Sale Sale { get; set; } = null!;
}
