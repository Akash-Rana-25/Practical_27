using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Core.Models;

namespace UnitOfWork.Infrastructure;

public class DbContextClass : DbContext
{
	public DbContextClass(DbContextOptions<DbContextClass> options) : base(options) { }

	public DbSet<Product> Products { get; set; } = null!;
	public DbSet<Category> Categories { get; set; } = null!;
}
