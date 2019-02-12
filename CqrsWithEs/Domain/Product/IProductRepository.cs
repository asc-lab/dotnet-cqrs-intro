using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CqrsWithEs.Domain.Product
{
    public interface IProductRepository
    {
        void Add(Domain.Product.Product product);

        Task<Domain.Product.Product> WithCode(string code);

        Task<ReadOnlyCollection<Domain.Product.Product>> All();
    }
}