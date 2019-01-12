namespace NoCqrs.Domain
{
    public interface IOfferRepository
    {
        Offer WithNumber(string number);

        void Add(Offer offer);
    }
}