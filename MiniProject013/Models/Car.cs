using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniProject013.Models
{
    [Table("MiniProject013_Cars")]
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Brand Name cannot be longer than 50 characters.")]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Brand Name cannot be longer than 50 characters.")]
        [DisplayName("Model Name")]
        public string ModelName { get; set; }

        public string? CarImage { get; set; }
    }
}
