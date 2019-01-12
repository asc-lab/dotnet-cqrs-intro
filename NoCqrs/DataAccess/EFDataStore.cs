using System.Security.Permissions;
using NoCqrs.Domain;

namespace NoCqrs.DataAccess
{
    public class EFDataStore : IDataStore
    {
        private readonly InsuranceDbContext dbContext;
        private readonly IOfferRepository offerRepository;
        private readonly IProductRepository productRepository;
        private readonly IPolicyRepository policyRepository;

        public IOfferRepository Offers => offerRepository;
        public IProductRepository Products => productRepository;
        public IPolicyRepository Policies => policyRepository;
        
        public EFDataStore(InsuranceDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.offerRepository = new EFOfferRepository(this.dbContext);
            this.productRepository = new EFProductRepository(this.dbContext);
            this.policyRepository = new EFPolicyRepository(this.dbContext);
        }

        public void CommitChanges()
        {
            dbContext.SaveChanges();
        }
    }
}