using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; }

        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }

        // navigation properties
        public virtual Invoice Invoice { get; set; }
    }
}
