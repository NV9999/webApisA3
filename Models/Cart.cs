using System.ComponentModel.DataAnnotations.Schema;
using WebAssignment3.Data;

namespace WebAssignment3.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey("Products")]
        public int ProductIds { get; set; }
        public int Quantities { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

    }
}
