using System;
using System.Collections.Generic;

namespace WebAssignment3.Data;

public partial class Comment
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? Rating { get; set; }

    public string? Image { get; set; }

    public string? Text { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
