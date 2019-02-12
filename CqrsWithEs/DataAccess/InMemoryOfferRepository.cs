using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CqrsWithEs.Domain.Offer;

namespace CqrsWithEs.DataAccess
{
    public class InMemoryOfferRepository : IOfferRepository
    {
        private readonly IDictionary<string, Offer> offers = new ConcurrentDictionary<string, Offer>();
        
        public Task<Offer> WithNumber(string number)
        {
            return Task.FromResult(offers[number]);
        }

        public Task<ReadOnlyCollection<Offer>> All()
        {
            return Task.FromResult(offers.Values.ToList().AsReadOnly());
        }

        public void Add(Offer offer)
        {
            offers.Add(offer.Number, offer);
        }
    }
}