using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class Sale
{

    public Guid SaleId { get; set; }

    public Guid? CustomerId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? SaleDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? Discount { get; set; }

    public decimal? NetAmount { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
