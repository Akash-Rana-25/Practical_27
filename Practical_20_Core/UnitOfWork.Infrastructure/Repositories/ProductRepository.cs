using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Core.Interfaces;
using UnitOfWork.Core.Models;

namespace UnitOfWork.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
	public ProductRepository(DbContextClass dbContextClass) : base(dbContextClass)
	{

	}
}
