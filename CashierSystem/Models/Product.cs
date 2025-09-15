using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class Product
{
    [NotMapped]
    public string Id { get; set; }
    public Guid ProductId { get; set; } = Guid.NewGuid();

    public string ProductName { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public Guid? SupplierId { get; set; }

    public string? Barcode { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SalePrice { get; set; }

    public int? ReorderLevel { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public virtual Supplier? Supplier { get; set; }
}
