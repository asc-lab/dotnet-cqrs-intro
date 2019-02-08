using System;
using CqrsWithEs.Domain;

namespace CqrsWithEs.Tests
{
    /*public static class PolicyTestData
    {
        public static Policy StandardOneYearPolicy(DateTime policyStartDate)
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(policyStartDate.AddDays(10));
            return Policy.ConvertOffer(offer, "POL0001", policyStartDate.AddDays(-1), policyStartDate);
        }

        public static Policy StandardOneYearPolicyTerminated(DateTime policyStartDate, DateTime policyTerminationDate)
        {
            var policy = StandardOneYearPolicy(policyStartDate);
            policy.TerminatePolicy(policyTerminationDate);
            policy.ConfirmChanges(2);

            return policy;
        }
    }*/
}