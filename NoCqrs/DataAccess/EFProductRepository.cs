using System.Linq;
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
            return dbContext.Products.FirstOrDefault(p => p.Code == code);
        }
    }
}