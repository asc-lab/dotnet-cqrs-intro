using System;
using System.Collections.Generic;

namespace SeparateModels.Domain
{
    public class ValidityPeriod : ValueObject<ValidityPeriod>,  ICloneable
    {
        public virtual DateTime ValidFrom { get; private set; }
        public virtual DateTime ValidTo { get; private set; }
    
        public ValidityPeriod(DateTime validFrom, DateTime validTo)
        {
            ValidFrom = validFrom;
            ValidTo = validTo;
        }
    
        protected ValidityPeriod()
        {
        } //NH required
    
        public static ValidityPeriod Between(DateTime validFrom, DateTime validTo)
            => new ValidityPeriod(validFrom, validTo);
    
        public ValidityPeriod Clone()
        {
            return new ValidityPeriod(ValidFrom, ValidTo);
        }
    
        public bool Contains(DateTime theDate)
        {
            if (theDate > ValidTo)
                return false;
    
            if (theDate < ValidFrom)
                return false;
    
            return true;
        }
    
        public ValidityPeriod EndOn(DateTime endDate)
        {
            return new ValidityPeriod(ValidFrom, endDate);
        }
    
        public int Days => ValidTo.Subtract(ValidFrom).Days;
    
        object ICloneable.Clone()
        {
            return Clone();
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object> { ValidFrom, ValidTo };
        }
    }
}