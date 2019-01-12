using System;
using NoCqrs.Domain;

namespace NoCqrs.Tests
{
    public class PolicyTestData
    {
        public static Policy StandardOneYearPolicy(DateTime policyStartDate)
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(policyStartDate.AddDays(10));
            return Policy.ConvertOffer(offer, "POL0001", policyStartDate.AddDays(-1), policyStartDate);
        }
    }
}