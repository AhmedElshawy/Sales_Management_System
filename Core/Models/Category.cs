using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        // navigation properties
        public virtual ICollection<Product> Products { get; set; }
    }
}
