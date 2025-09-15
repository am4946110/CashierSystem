using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class VwCurrentStock
{
    [NotMapped]
    public string Id { get; set; }
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Barcode { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? SupplierName { get; set; }

    public int? CurrentStock { get; set; }

    public int? ReorderLevel { get; set; }
}
