
using System.ComponentModel.DataAnnotations;

namespace Practical_20_middlewares.Dto;

public class CreateCategoryDto
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string? Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }
}
