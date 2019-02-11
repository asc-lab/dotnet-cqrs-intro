using CqrsWithEs.Domain.Base;

namespace CqrsWithEs.Domain.Policy.Events
{
    public class CoverageExtendedPolicyVersionConfirmed : Event
    {
        public int VersionNumber { get; private set; }
        public PolicyVersionStatus VersionStatus { get; private set; }

        public CoverageExtendedPolicyVersionConfirmed(int versionNumber)
        {
            VersionNumber = versionNumber;
            VersionStatus = PolicyVersionStatus.Active;
        }
    }
}