using System;
using System.Collections.Generic;

namespace WebAssignment3.Data;

public partial class User
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Username { get; set; }

    public string? PurchaseHistory { get; set; }

    public string? ShippingAddress { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
