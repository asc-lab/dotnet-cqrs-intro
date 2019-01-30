using System;
using NodaMoney;

namespace SeparateModels.Domain
{
    public class UnitPrice
    {
        public Money Value { get; private set; }
        public TimeSpan Unit { get; private set; }

        public UnitPrice(Money value, TimeSpan unit)
        {
            Value = value;
            Unit = unit;
        }

        public Money Multiply(TimeSpan qt)
        {
            return (qt.Days / qt.Days) * Value;
        }
    }
}