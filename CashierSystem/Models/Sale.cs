using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class Sale
{
    [NotMapped]
    public string Id { get; set; }

    public Guid SaleId { get; set; } = Guid.NewGuid();

    public Guid? CustomerId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? SaleDate { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? Discount { get; set; }

    public decimal? NetAmount { get; set; }

    public virtual Customer? Customer { get; set; }
    [NotMapped]
    public virtual MyUser? User { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
