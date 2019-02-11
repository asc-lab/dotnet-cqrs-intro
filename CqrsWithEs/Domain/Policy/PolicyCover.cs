using System;
using CqrsWithEs.Domain.Common;
using CqrsWithEs.Domain.Offer;
using NodaMoney;

namespace CqrsWithEs.Domain.Policy
{
    public class PolicyCover
    {
        public string CoverCode { get; }
        public ValidityPeriod CoverPeriod { get; }
        public UnitPrice Price { get; }
        public Money Amount { get; }

        public PolicyCover(string coverCode, ValidityPeriod coverPeriod, UnitPrice price)
        {
            CoverCode = coverCode;
            CoverPeriod = coverPeriod;
            Price = price;
            Amount = CalculateAmount();
        }

        public static PolicyCover ForPrice(CoverPrice coverPrice, ValidityPeriod coverPeriod)
        {
            return new PolicyCover
            (
                coverPrice.CoverCode,
                coverPeriod,
                new UnitPrice(coverPrice.Price, coverPrice.CoverPeriod)
            );
        }

        public PolicyCover EndOn(DateTime lastDateOfCover)
        {
            return new PolicyCover
            (
                CoverCode,
                CoverPeriod.EndOn(lastDateOfCover),
                Price
            );
        }

        private Money CalculateAmount()
        {
            return decimal.Divide(CoverPeriod.Days, Price.PricePeriod.Days) * Price.Price;
        }
    }
}