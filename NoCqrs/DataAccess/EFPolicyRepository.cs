using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            return dbContext
                .Policies
                .Include(p => p.Product)
                .Include(p => p.Versions)
                .ThenInclude(pv => pv.Covers)
                .ThenInclude(c => c.Cover)
                .FirstOrDefault(p => p.Number == number);
        }

        public void Add(Policy policy)
        {
            dbContext.Policies.Add(policy);
        }

        public IList<Policy> Find(PolicyFilter filter)
        {
            throw new System.NotImplementedException();
        }
    }
}