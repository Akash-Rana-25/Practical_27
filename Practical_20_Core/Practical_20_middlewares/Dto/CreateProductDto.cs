

using Practical_20_middlewares.Middlewares;
using System.ComponentModel.DataAnnotations;
namespace Practical_20_middlewares.Dto;
public class CreateProductDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(typeof(decimal), "1", "79228162514264337593543950335")]
    public decimal Price { get; set; }

    [Required]
    [Range(1, 1000)]
    public int Stock { get; set; }

    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
}
