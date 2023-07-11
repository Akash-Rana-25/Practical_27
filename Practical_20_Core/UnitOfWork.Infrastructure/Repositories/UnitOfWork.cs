using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Core.Interfaces;

namespace UnitOfWork.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContextClass _dbContextClass;
    public IProductRepository Products { get; }
    public ICategoryRepository Categories { get; }

    public UnitOfWork(DbContextClass dbContextClass, IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _dbContextClass = dbContextClass;
        Products = productRepository;
        Categories = categoryRepository;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<int> Save()
    {
        return await _dbContextClass.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContextClass.Dispose();
        }
    }
}
