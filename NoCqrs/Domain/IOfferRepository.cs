using System.Collections.Generic;

namespace NoCqrs.Domain
{
    public interface IOfferRepository
    {
        Offer WithNumber(string number);

        List<Offer> All();

        void Add(Offer offer);
    }
}