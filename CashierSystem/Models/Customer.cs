using System;
using System.Collections.Generic;

namespace CashierSystem.Models;

public partial class Customer
{

    public Guid CustomerId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateBecameCustomer { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
