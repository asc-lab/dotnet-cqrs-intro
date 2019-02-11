using System.Collections.Generic;
using System.Linq;

namespace CqrsWithEs.Domain.Base
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

        public override bool Equals(object other)
        {
            return Equals(other as T);
        }

        public override int GetHashCode()
        {
            return GetAttributesToIncludeInEqualityCheck().Aggregate(17, (current, obj) => current * 31 + (obj == null ? 0 : obj.GetHashCode()));
        }

        public bool Equals(T other)
        {
            if (other == null) return false;

            return GetAttributesToIncludeInEqualityCheck().SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public static bool operator ==(ValueObject<T> left, ValueObject<T> right)
        {
            return Equals(left, right);
        }
        
        public static bool operator !=(ValueObject<T> left, ValueObject<T> right)
        {
            return !(left == right);
        }
    }
}