using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class Customer
{
    [NotMapped]
    public string Id { get; set; }
    public Guid CustomerId { get; set; } = Guid.NewGuid();

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateBecameCustomer { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
