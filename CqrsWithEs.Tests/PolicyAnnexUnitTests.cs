using System;
using System.Linq;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Offer;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Policy.Events;
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
            Equal(2, policy.Versions.Count());
            Equal(Money.Euro(500), policy.Versions.WithNumber(1).TotalPremium);
            Equal(PolicyVersionStatus.Active, policy.Versions.WithNumber(1).VersionStatus);
            Equal(Money.Euro(550.41), policy.Versions.WithNumber(2).TotalPremium);
            Equal(PolicyVersionStatus.Draft, policy.Versions.WithNumber(2).VersionStatus);
            Equal(PolicyVersionStatus.Draft, policy.Versions.WithNumber(2).VersionStatus);
            
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