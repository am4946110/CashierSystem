using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class VwSalesDetail
{
    [NotMapped]
    public string Id { get; set; }
    public Guid SaleId { get; set; }

    public DateTime? SaleDate { get; set; }

    public string? CustomerName { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? Discount { get; set; }

    public decimal? NetAmount { get; set; }
}
