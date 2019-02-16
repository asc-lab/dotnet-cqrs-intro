using System;
using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Policy.Events;
using CqrsWithEs.Tests.Asserts;
using KellermanSoftware.CompareNetObjects;
using NodaMoney;
using Xunit;
using Xunit.Asserts.Compare;
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
            resultingEvents
                .Should()
                .BeSingle()
                .ContainEvent(
                    new TerminalPolicyVersionCreated
                    (
                        2,
                        1,
                        new DateTime(2019,7,1), 
                        new DateTime(2020,1,1),
                        new DateTime(2019,1,1), 
                        new DateTime(2019,6,30),
                        new List<PolicyCoverData>
                        {
                            new PolicyCoverData("OC",new DateTime(2019,1,1),new DateTime(2019,6,30),Money.Euro(246.58),Money.Euro(500),TimeSpan.FromDays(365))
                        }
                    )
                );
        }

    }
}