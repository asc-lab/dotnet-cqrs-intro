using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NoCqrs.Domain
{
    public static class CoversList
    {
        public static Cover WithCode(this IEnumerable<Cover> covers, string code)
        {
            return covers?.FirstOrDefault(c => c.Code == code);
        }
    }
}