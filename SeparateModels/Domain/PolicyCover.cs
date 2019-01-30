using System;
using NodaMoney;

namespace SeparateModels.Domain
{
    public class PolicyCover
    {
        public Guid Id { get; private set; }
        public Cover Cover { get; private set; }
        public ValidityPeriod CoverPeriod { get; private set; }
        
        public Money Price { get; private set; }
        public TimeSpan PricePeriod { get; private set; }
        
        public Money Amount { get; private set; }

        public PolicyCover(Guid id, Cover cover, ValidityPeriod coverPeriod, Money price, TimeSpan pricePeriod)
        {
            Id = id;
            Cover = cover;
            CoverPeriod = coverPeriod;
            Price = price;
            PricePeriod = pricePeriod;
            Amount = CalculateAmount();
        }

        // required by EF
        protected PolicyCover()
        {
        }
        
        
        public void EndCoverOn(DateTime lastDayOfCover)
        {
            CoverPeriod = CoverPeriod.EndOn(lastDayOfCover);
            Amount = CalculateAmount();
        }

        private Money CalculateAmount()
        {
            return decimal.Divide(CoverPeriod.Days, PricePeriod.Days) * Price;
        }

    }
}