using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierSystem.Models;

public partial class Category
{
    [NotMapped]
    public string Id { get; set; }
    public Guid CategoryId { get; set; } = Guid.NewGuid();

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
