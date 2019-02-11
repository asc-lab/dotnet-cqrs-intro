using CqrsWithEs.Domain.Base;

namespace CqrsWithEs.Domain.Policy.Events
{
    public class TerminalPolicyVersionConfirmed : Event
    {
        public int VersionNumber { get; private set; }
        public PolicyVersionStatus VersionStatus { get; private set; }

        public TerminalPolicyVersionConfirmed(int versionNumber)
        {
            VersionNumber = versionNumber;
            VersionStatus = PolicyVersionStatus.Active;
        }
    }
}