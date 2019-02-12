using System;
using System.Linq;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Offer;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Policy.Events;
using CqrsWithEs.Tests.Asserts;
using NodaMoney;
using Xunit;
using static Xunit.Assert;

namespace CqrsWithEs.Tests
{
    public class PolicyAnnexUnitTests
    {
        [Fact]
        public void CanExtendCoverageWithMiddleDayOfPolicy()
        {
            var product = ProductsTestData.StandardCarInsurance();
            var policy = PolicyTestData.StandardOneYearPolicy(new DateTime(2019, 1, 1));

            var newCover = product.CoverWithCode("AC");
            policy.ExtendCoverage
            (
                new DateTime(2019, 7, 1), 
                new CoverPrice(Guid.NewGuid(),newCover, Money.Euro(100) , TimeSpan.FromDays(365))
            );
            
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
                .HaveActivePolicyStatus()
                .CoverPeriod(new DateTime(2019, 1, 1), new DateTime(2020,1,1))
                .HaveTotalPremiumEqualTo(Money.Euro(550.41));
            
            
            //assert events
            Single(resultingEvents);
            IsType<CoverageExtendedPolicyVersionCreated>(resultingEvents.First());
        }

        [Fact]
        public void CanAddTheSameCoverTwice()
        {
            var product = ProductsTestData.StandardCarInsurance();
            var policy = PolicyTestData.StandardOneYearPolicy(new DateTime(2019, 1, 1));

            var doubledCover = product.CoverWithCode("OC");
            var ex = Throws<ApplicationException>(() => policy.ExtendCoverage
            (
                new DateTime(2019, 7, 1), 
                new CoverPrice(Guid.NewGuid(),doubledCover, Money.Euro(100) , TimeSpan.FromDays(365))
            ));
            Equal("This cover is already present", ex.Message);
        }
    }
}