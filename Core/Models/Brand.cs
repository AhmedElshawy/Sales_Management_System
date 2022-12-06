using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        //navigation properties
        public virtual ICollection<Product> Products { get; set; }
    }
}
