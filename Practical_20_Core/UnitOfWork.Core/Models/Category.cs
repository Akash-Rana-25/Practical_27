using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitOfWork.Core.Models;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column(TypeName = "VARCHAR(50)")]
    public string? Name { get; set; }

    [Column(TypeName = "VARCHAR(500)")]
    public string? Description { get; set; }
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
}
