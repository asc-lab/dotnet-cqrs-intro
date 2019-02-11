using System.Collections.Generic;
using System.Threading.Tasks;

namespace CqrsWithEs.Domain.Offer
{
    public interface IOfferRepository
    {
        Task<Offer> WithNumber(string number);

        Task<IReadOnlyList<Offer>> All();

        void Add(Offer offer);
    }
}