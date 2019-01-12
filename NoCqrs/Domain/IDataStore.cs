namespace NoCqrs.Domain
{
    public interface IDataStore
    {
        IOfferRepository Offers { get; }
        IPolicyRepository Policies { get; }
        void CommitChanges();
    }
}