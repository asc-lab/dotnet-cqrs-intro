using MediatR;

namespace SeparateModels.Domain
{
    public class PolicyCreated : INotification
    {
        public Policy NewPolicy { get; }

        public PolicyCreated(Policy newPolicy)
        {
            NewPolicy = newPolicy;
        }
    }

    public class PolicyAnnexed : INotification
    {
        public Policy AnnexedPolicy { get; }
        public PolicyVersion AnnexVersion { get; }

        public PolicyAnnexed(Policy annexedPolicy, PolicyVersion annexVersion)
        {
            AnnexedPolicy = annexedPolicy;
            AnnexVersion = annexVersion;
        }
    }

    public class PolicyTerminated : INotification
    {
        public Policy TerminatedPolicy { get; }
        public PolicyVersion TerminatedVersion { get; }

        public PolicyTerminated(Policy terminatedPolicy, PolicyVersion terminatedVersion)
        {
            TerminatedPolicy = terminatedPolicy;
            TerminatedVersion = terminatedVersion;
        }
    }

    public class PolicyAnnexCancelled : INotification
    {
        public Policy Policy { get; }
        public PolicyVersion CancelledAnnexVersion { get; }
        public PolicyVersion CurrentVersionAfterAnnexCancellation { get; }

        public PolicyAnnexCancelled(
            Policy policy, 
            PolicyVersion cancelledAnnexVersion,
            PolicyVersion currentVersionAfterAnnexCancellation)
        {
            Policy = policy;
            CancelledAnnexVersion = cancelledAnnexVersion;
            CurrentVersionAfterAnnexCancellation = currentVersionAfterAnnexCancellation;
        }
    }
}