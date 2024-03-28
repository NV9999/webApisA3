using System;
using System.Collections.Generic;

namespace WebAssignment3.Data;

public partial class Order
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal? Total { get; set; }

    public virtual Cart? Cart { get; set; }
}
