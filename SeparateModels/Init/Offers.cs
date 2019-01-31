using System;
using System.Collections.Generic;
using NodaMoney;
using SeparateModels.Domain;

namespace SeparateModels.Init
{
    public class Offers
    {
        public static Offer StandardOneYearOCOfferValidUntil(Product product, string number, DateTime validityEnd)
        {
            var coverPrices = new Dictionary<Cover, Money>()
            {
                {product.Covers.WithCode("OC"), Money.Euro(500)}
            };
            
            return new Offer
            (
                Guid.NewGuid(), 
                number,
                product,
                Persons.Kowalski(),
                Persons.Kowalski(),
                Cars.OldFordFocus(),
                TimeSpan.FromDays(365), 
                Money.Euro(500),
                validityEnd.AddDays(-30),
                coverPrices
            );
        }

        public static Offer RejectedOfferValidUntil(Product product, string number, DateTime validityEnd)
        {
            var offer = StandardOneYearOCOfferValidUntil(product, number, validityEnd);
            offer.Reject();
            return offer;
        }
        
        public static Offer ConvertedOfferValidUntil(Product product, string number ,DateTime validityEnd)
        {
            var offer = StandardOneYearOCOfferValidUntil(product, number, validityEnd);
            offer.Convert();
            return offer;
        }
    }
}