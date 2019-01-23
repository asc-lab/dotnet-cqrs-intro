using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NoCqrs.Domain
{
    public static class PolicyVersions
    {
        public static PolicyVersion EffectiveAtDate(this IEnumerable<PolicyVersion> versions, DateTime effectiveDate)
        {
            return versions?
                .Where(v => v.IsEffectiveOn(effectiveDate) && !v.IsDraft())
                .OrderByDescending(v => v.VersionNumber)
                .FirstOrDefault();
        }

        public static int MaxVersionNumber(this IEnumerable<PolicyVersion> versions)
        {
            return versions.Max(v => v.VersionNumber);
        }

        public static PolicyVersion WithNumber(this IEnumerable<PolicyVersion> versions, int versionNumber)
        {
            return versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
        }
        
        public static PolicyVersion LatestActive(this IEnumerable<PolicyVersion> versions)
        {
            return versions
                .Where(v => v.VersionStatus == PolicyVersionStatus.Active)
                .OrderByDescending(v => v.VersionNumber)
                .FirstOrDefault();
        }
    }
}