using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWork.Core.Models;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column(TypeName = "VARCHAR(50)")]
    public string? Name { get; set; }

    [Column(TypeName = "VARCHAR(500)")]
    public string? Description { get; set; }
    public decimal Price { get; set; }

    [Range(0,1000)]
    public int Stock { get; set; }

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}
