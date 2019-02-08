using System;
using System.Linq;
using CqrsWithEs.Domain;
using NodaMoney;
using Xunit;
using static Xunit.Assert;

namespace CqrsWithEs.Tests
{
    public class PolicyTerminationUnitTests
    {
        [Fact]
        public void CanTerminatePolicyInTheMiddle()
        {
            var policy = PolicyTestData.StandardOneYearPolicy(new DateTime(2019, 1, 1));
            var terminationDate = new DateTime(2019, 7, 1);
            
            policy.TerminatePolicy(terminationDate);
            var resultingEvents = policy.GetUncommittedChanges();
            
            //assert state
            Equal(2, policy.Versions.Count());
            Equal(Money.Euro(500), policy.Versions.WithNumber(1).TotalPremium);
            Equal(PolicyVersionStatus.Active, policy.Versions.WithNumber(1).PolicyVersionStatus);
            
            Equal(PolicyStatus.Terminated, policy.Versions.WithNumber(2).PolicyStatus);
            Equal(PolicyVersionStatus.Draft, policy.Versions.WithNumber(2).PolicyVersionStatus);
            Equal(terminationDate, policy.Versions.WithNumber(2).VersionPeriod.ValidFrom);
            Equal(terminationDate.AddDays(-1), policy.Versions.WithNumber(2).CoverPeriod.ValidTo);
            Equal(new DateTime(2019,1,1), policy.Versions.WithNumber(2).CoverPeriod.ValidFrom);
            Equal(Money.Euro(246.58), policy.Versions.WithNumber(2).TotalPremium);
            
            //assert events
            Single(resultingEvents);
            IsType<TerminalPolicyVersionCreated>(resultingEvents.First());
        }
    }
}