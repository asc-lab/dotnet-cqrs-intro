using System;
using System.Collections.Generic;
using CqrsWithEs.Domain;
using NodaMoney;

namespace CqrsWithEs.Tests
{
    public static class PolicyTestData
    {
        public static Policy StandardOneYearPolicy(DateTime policyStartDate)
        {
            var product = ProductsTestData.StandardCarInsurance(); 
            
            var events = new List<Event>
            {
                new InitialPolicyVersionCreated
                (
                    "POL01",
                    product.Code,
                    policyStartDate,
                    policyStartDate.AddDays(365),
                    policyStartDate.AddDays(-1),
                    new PersonData("A", "B", "C"),
                    new CarData("A","B", 2018),
                    new List<PolicyCoverData>
                    {
                        new PolicyCoverData("OC",policyStartDate,policyStartDate.AddDays(365),Money.Euro(500),Money.Euro(500),TimeSpan.FromDays(365))
                    }
                )
            };
            
            var policy = new Policy(Guid.NewGuid(), events);
            policy.MarkChangesAsCommitted();
            return policy;
        }

        /*public static Policy StandardOneYearPolicyTerminated(DateTime policyStartDate, DateTime policyTerminationDate)
        {
            var policy = StandardOneYearPolicy(policyStartDate);
            policy.TerminatePolicy(policyTerminationDate);
            policy.ConfirmChanges(2);

            return policy;
        }*/
    }
}