using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeparateModels.Domain
{
    public interface IProductRepository
    {
        void Add(Product product);

        Task<Product> WithCode(string code);

        Task<IReadOnlyList<Product>> All();
    }
}