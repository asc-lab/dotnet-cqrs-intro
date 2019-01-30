using System.Collections.Generic;
using System.Linq;

namespace SeparateModels.Domain
{
    public static class CoversList
    {
        public static Cover WithCode(this IEnumerable<Cover> covers, string code)
        {
            return covers?.FirstOrDefault(c => c.Code == code);
        }
    }
}