using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CqrsWithEs.Domain.Product;

namespace CqrsWithEs.DataAccess
{
    public class InMemoryProductsRepository : IProductRepository
    {
        private readonly IDictionary<string, Product> products = new ConcurrentDictionary<string, Product>();
        
        public void Add(Product product)
        {
            products.Add(product.Code, product);
        }

        public Task<Product> WithCode(string code)
        {
            return Task.FromResult(products[code]);
        }

        public Task<ReadOnlyCollection<Product>> All()
        {
            var allProducts = products.Values.ToList().AsReadOnly();
            return Task.FromResult(allProducts);
        }
    }
}