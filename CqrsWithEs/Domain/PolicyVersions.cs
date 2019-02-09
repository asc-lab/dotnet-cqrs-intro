using System;
using System.Collections.Generic;
using System.Linq;

namespace CqrsWithEs.Domain
{
    public static class PolicyVersions
    {
        public static PolicyVersion EffectiveAt(this IEnumerable<PolicyVersion> versions, DateTime effectiveDateOfChange)
        {
            return versions
                .Where(v => v.VersionStatus == PolicyVersionStatus.Active)
                .Where(v => v.IsEffectiveOn(effectiveDateOfChange))
                .OrderByDescending(v => v.VersionNumber)
                .FirstOrDefault();
        }

        public static PolicyVersion WithNumber(this IEnumerable<PolicyVersion> versions, int versionNumber)
        {
            return versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
        }
    }
}