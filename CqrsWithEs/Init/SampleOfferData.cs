using System;
using System.Collections.Generic;
using CqrsWithEs.Domain.Common;
using CqrsWithEs.Domain.Offer;
using CqrsWithEs.Domain.Product;
using NodaMoney;

namespace CqrsWithEs.Init
{
    public class SampleOfferData
    {
        public static Offer SampleOffer(string offerNumber)
        {
            return new Offer
            (
                Guid.NewGuid(),
                offerNumber,
                new Product(Guid.NewGuid(),"OC", "OC", new List<Cover> {}),
                new Person("Jan", "Nowak", "11111111116"),
                new Person("Jan", "Nowak", "11111111116"),
                new Car("Ford Escort","WAW1010",2001),
                TimeSpan.FromDays(365),
                Money.Euro(350),
                new DateTime(2019,2,25), 
                new Dictionary<Cover, Money>
                {
                    {new Cover(Guid.NewGuid(),"OC","OC"), Money.Euro(350)} 
                }
            );
        }
    }
}