using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CqrsWithEs.Domain.Offer
{
    public interface IOfferRepository
    {
        Task<Offer> WithNumber(string number);

        Task<ReadOnlyCollection<Offer>> All();

        void Add(Offer offer);
    }
}