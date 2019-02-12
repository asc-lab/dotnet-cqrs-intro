using System;
using System.Linq;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Policy.Events;
using CqrsWithEs.Tests.Asserts;
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
            policy
                .Should()
                .HaveVersions(2);
                
            policy.Versions.WithNumber(1).Should()
                .BeActive()
                .HaveActivePolicyStatus()
                .HaveTotalPremiumEqualTo(Money.Euro(500));
            
            policy.Versions.WithNumber(2)
                .Should()
                .BeDraft()
                .HaveTerminatedPolicyStatus()
                .CoverPeriod(new DateTime(2019,1,1), terminationDate.AddDays(-1))
                .HaveTotalPremiumEqualTo(Money.Euro(246.58));
           
            
            //assert events
            Single(resultingEvents);
            IsType<TerminalPolicyVersionCreated>(resultingEvents.First());
        }
    }
}