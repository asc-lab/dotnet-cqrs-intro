using System;
using System.Collections.Generic;
using NoCqrs.Domain;
using NodaMoney;
using NodaTime;

namespace NoCqrs.Tests
{
    public class OffersTestData
    {
        public static Offer StandardOneYearOCOfferValidUntil(DateTime validityEnd)
        {
            var product = ProductsTestData.StandardCarInsurance(); 
            return new Offer
            (
                Guid.NewGuid(), 
                "1",
                product,
                PersonsTestData.Kowalski(),
                PersonsTestData.Kowalski(),
                CarsTestData.OldFordFocus(),
                TimeSpan.FromDays(365), 
                Money.Euro(500),
                validityEnd.AddDays(-30),
                new Dictionary<Cover, Money>()
                {
                    {product.Covers.WithCode("OC"), Money.Euro(500) }
                }
            );
        }

        public static Offer RejectedOfferValidUntil(DateTime validityEnd)
        {
            var offer = StandardOneYearOCOfferValidUntil(validityEnd);
            offer.Reject();
            return offer;
        }
        
        public static Offer ConvertedOfferValidUntil(DateTime validityEnd)
        {
            var offer = StandardOneYearOCOfferValidUntil(validityEnd);
            offer.Convert();
            return offer;
        }
    }
}