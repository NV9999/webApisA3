using System.ComponentModel.DataAnnotations.Schema;

namespace WebAssignment3.Models
{
    public class Comments
    {
        public int Id { get; set; }
        [ForeignKey("Products")]
        public int ProductId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
    }
}
