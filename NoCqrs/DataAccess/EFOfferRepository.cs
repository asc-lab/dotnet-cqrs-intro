using System.Linq;
using NoCqrs.Domain;

namespace NoCqrs.DataAccess
{
    public class EFOfferRepository : IOfferRepository
    {
        private readonly InsuranceDbContext dbContext;

        public EFOfferRepository(InsuranceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Offer WithNumber(string number)
        {
            return dbContext.Offers.FirstOrDefault(o => o.Number == number);
        }

        public void Add(Offer offer)
        {
            dbContext.Offers.Add(offer);
        }
    }
}