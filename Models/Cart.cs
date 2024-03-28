using System.ComponentModel.DataAnnotations.Schema;

namespace WebAssignment3.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [ForeignKey("Products")]
        public int ProductIds { get; set; }
        public string Quantities { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
    }
}
