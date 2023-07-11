
using Practical_20_middlewares.Middlewares;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Practical_20_middlewares.Dto;
public class UpdateProductDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Range(typeof(decimal), "1", "79228162514264337593543950335")]
    public decimal Price { get; set; }

    [Range(1, 1000)]
    public int Stock { get; set; }

    [GuidNotEmpty]
    public Guid CategoryId { get; set; }
}
