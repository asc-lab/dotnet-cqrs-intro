using System.Collections.Generic;
using NoCqrs.Domain;

namespace NoCqrs.DataAccess
{
    public class EFPolicyRepository : IPolicyRepository
    {
        private readonly InsuranceDbContext dbContext;
        
        public EFPolicyRepository(InsuranceDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Policy WithNumber(string number)
        {
            throw new System.NotImplementedException();
        }

        public void Add(Policy policy)
        {
            throw new System.NotImplementedException();
        }

        public IList<Policy> Find(PolicyFilter filter)
        {
            throw new System.NotImplementedException();
        }
    }
}