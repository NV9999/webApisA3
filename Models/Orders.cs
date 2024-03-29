using System.ComponentModel.DataAnnotations.Schema;

namespace WebAssignment3.Models
{
    public class Orders
    {
        public int Id { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
    }
}
