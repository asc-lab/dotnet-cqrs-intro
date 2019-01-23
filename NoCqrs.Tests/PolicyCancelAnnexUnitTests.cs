using System;
using System.Linq;
using NoCqrs.Domain;
using NodaMoney;
using Xunit;
using static Xunit.Assert;

namespace NoCqrs.Tests
{
    public class PolicyCancelAnnexUnitTests
    {
        [Fact]
        public void CanUndoTermination()
        {
            var policyStartedAt = new DateTime(2019, 1, 1);
            var policyTerminatedAt = new DateTime(2019,7,1);
            var policy = PolicyTestData.StandardOneYearPolicyTerminated(policyStartedAt, policyTerminatedAt);

            policy.CancelLastAnnex();
            
            Equal(2, policy.Versions.Count());
            Equal(Money.Euro(500), policy.Versions.WithNumber(1).TotalPremium);
            Equal(PolicyVersionStatus.Active, policy.Versions.WithNumber(1).VersionStatus);
            Equal(PolicyStatus.Active, policy.Versions.WithNumber(1).PolicyStatus);
            
            Equal(PolicyStatus.Terminated, policy.Versions.WithNumber(2).PolicyStatus);
            Equal(PolicyVersionStatus.Cancelled, policy.Versions.WithNumber(2).VersionStatus);
            Equal(policyTerminatedAt , policy.Versions.WithNumber(2).VersionValidityPeriod.ValidFrom);
            Equal(policyTerminatedAt.AddDays(-1), policy.Versions.WithNumber(2).CoverPeriod.ValidTo);
            Equal(new DateTime(2019,1,1), policy.Versions.WithNumber(2).CoverPeriod.ValidFrom);
            Equal(Money.Euro(246.58), policy.Versions.WithNumber(2).TotalPremium);

        }
    }
}