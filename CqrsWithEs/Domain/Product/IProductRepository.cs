using System.Collections.Generic;
using System.Threading.Tasks;

namespace CqrsWithEs.Domain.Product
{
    public interface IProductRepository
    {
        void Add(Domain.Product.Product product);

        Task<Domain.Product.Product> WithCode(string code);

        Task<IReadOnlyList<Domain.Product.Product>> All();
    }
}