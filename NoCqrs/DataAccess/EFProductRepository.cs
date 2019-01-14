using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoCqrs.Domain;

namespace NoCqrs.DataAccess
{
    public class EFProductRepository : IProductRepository
    {
        private readonly InsuranceDbContext dbContext;

        public EFProductRepository(InsuranceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Product product)
        {
            dbContext.Products.Add(product);
        }

        public Product WithCode(string code)
        {
            return dbContext.Products.Include(p => p.Covers).FirstOrDefault(p => p.Code == code);
        }

        public List<Product> All()
        {
            return dbContext.Products.Include(p => p.Covers).ToList();
        }
    }
}