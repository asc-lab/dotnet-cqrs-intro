using System;
using System.Linq;
using NoCqrs.Domain;
using NodaMoney;
using Xunit;
using static Xunit.Assert;

namespace NoCqrs.Tests
{
    public class PolicyAnnexesUnitTest
    {
        [Fact]
        public void CanExtendCoverageWithFirstDayOfPolicy()
        {
            
        }
        
        [Fact]
        public void CanExtendCoverageWithLastDayOfPolicy()
        {
        }
        
        [Fact]
        public void CanExtendCoverageWithMiddleDayOfPolicy()
        {
            var product = ProductsTestData.StandardCarInsurance();
            var policy = PolicyTestData.StandardOneYearPolicy(new DateTime(2019, 1, 1));

            var newCover = product.Covers.WithCode("AC");
            policy.ExtendCoverage
            (
                new DateTime(2019, 7, 1), 
                new CoverPrice(Guid.NewGuid(),newCover, Money.Euro(100) , TimeSpan.FromDays(365))
            );
            
            Equal(2, policy.Versions.Count());
            Equal(Money.Euro(500), policy.Versions.WithNumber(1).TotalPremium);
            Equal(PolicyVersionStatus.Active, policy.Versions.WithNumber(1).Status);
            Equal(Money.Euro(550.41), policy.Versions.WithNumber(2).TotalPremium);
            Equal(PolicyVersionStatus.Draft, policy.Versions.WithNumber(2).Status);
        }
        
        [Fact]
        public void CanExtendCoverageOverridingPreviousExtension()
        {
        }
        
        [Fact]
        public void CanExtendCoverageAddingToPreviousExtension()
        {
        }
        
        [Fact]
        public void CannotExtendCoverageOnTerminatedPolicy()
        {
        }
        
        [Fact]
        public void CannotExtendCoverageWithEffectiveDateBeforePolicyStart()
        {
        }
        
        [Fact]
        public void CannotExtendCoverageWithEffectiveDateAfterPolicyEnd()
        {
        }
    }
}