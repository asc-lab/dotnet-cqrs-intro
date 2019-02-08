using System;
using NodaMoney;

namespace CqrsWithEs.Domain
{
    public class UnitPrice
    {
        public Money Price { get; private set; }
        public TimeSpan PricePeriod { get; private set; }

        public UnitPrice(Money price, TimeSpan pricePeriod)
        {
            Price = price;
            PricePeriod = pricePeriod;
        }
    }
}