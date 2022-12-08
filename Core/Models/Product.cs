using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("Category")]
        [Required(ErrorMessage = "You have not chosen the category")]
        public int CategoryId { get; set; }

        [ForeignKey("Brand")]
        [Required(ErrorMessage ="You have not chosen the brand")]
        public int BrandId { get; set; }

        // navigation properties
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }

    }
}
