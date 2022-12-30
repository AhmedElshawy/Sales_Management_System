using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class AdditionOrder
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; } = DateTime.Now;

        public int AddedQunatity { get; set; }

        [ForeignKey("Product")]
        public int ProdutcId { get; set; }

        // Navigation props
        public Product Product { get; set; }
    }
}
