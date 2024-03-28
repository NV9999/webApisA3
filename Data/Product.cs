using System;
using System.Collections.Generic;

namespace WebAssignment3.Data;

public partial class Product
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public decimal? Pricing { get; set; }

    public decimal? ShippingCost { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
