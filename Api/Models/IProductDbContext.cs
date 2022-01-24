using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api.Models
{
    public interface IProductDbContext : IDisposable
    {
        DbSet<Product> Products { get; }
        
    }
}