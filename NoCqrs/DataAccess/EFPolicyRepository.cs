using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoCqrs.Domain;
using NoCqrs.Utils;

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
            var query = dbContext.Policies.AsQueryable();

            if (filter.PolicyNumber.NotBlank())
            {
                query = query.Where(p => p.Number == filter.PolicyNumber);
            }

            if (filter.CarPlateNumber.NotBlank())
            {
                query = query.Where(p => p.CurrentVersion.Car.PlateNumber == filter.CarPlateNumber);
            }
            
            if (filter.PolicyHolderFirstName.NotBlank())
            {
                query = query.Where(p => p.CurrentVersion.PolicyHolder.FirstName == filter.PolicyHolderFirstName);
            }
            
            if (filter.PolicyHolderLastName.NotBlank())
            {
                query = query.Where(p => p.CurrentVersion.PolicyHolder.LastName== filter.PolicyHolderLastName);
            }

            if (filter.PolicyStartDateFrom.HasValue)
            {
                query = query.Where(p => p.CurrentVersion.CoverPeriod.ValidFrom >= filter.PolicyStartDateFrom.Value);
            }

            if (filter.PolicyStartDateTo.HasValue)
            {
                query = query.Where(p => p.CurrentVersion.CoverPeriod.ValidFrom <= filter.PolicyStartDateTo.Value);
            }
            
            query = query
                .Include(p => p.Product)
                .Include(p => p.Versions)
                .ThenInclude(pv => pv.Covers)
                .ThenInclude(c => c.Cover);

            return query.ToList();
        }
    }
}