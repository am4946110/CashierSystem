using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class VwCurrentStock
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Barcode { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? SupplierName { get; set; }

    public int? CurrentStock { get; set; }

    public int? ReorderLevel { get; set; }
}
