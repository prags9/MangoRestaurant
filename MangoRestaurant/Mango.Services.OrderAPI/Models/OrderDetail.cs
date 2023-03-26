using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrdertHeaderId { get; set; }
        [ForeignKey("OrdertHeaderId")]
        public virtual OrderHeader OrderHeader { get; set; }

        public int ProductId { get; set; }
        public string Productname { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
