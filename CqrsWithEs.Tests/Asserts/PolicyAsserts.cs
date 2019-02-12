using System;
using CqrsWithEs.Domain.Policy;
using NodaMoney;
using static Xunit.Assert;

namespace CqrsWithEs.Tests.Asserts
{
    public class PolicyAssert
    {
        private readonly Policy policy;

        public PolicyAssert(Policy policy)
        {
            this.policy = policy;
        }

        public PolicyAssert HaveVersions(int expectedNumber)
        {
            Equal(expectedNumber, policy.Versions.Count);
            return this;
        }

        public PolicyVersionAssert VersionShould(int versionNumber)
        {
            return new PolicyVersionAssert(policy.Versions.WithNumber(versionNumber));
        }
    }


    public class PolicyVersionAssert
    {
        private readonly PolicyVersion version;

        public PolicyVersionAssert(PolicyVersion version)
        {
            this.version = version;
        }

        public PolicyVersionAssert HaveActivePolicyStatus()
        {
            Equal(PolicyStatus.Active, version.PolicyStatus);
            return this;
        }

        public PolicyVersionAssert HaveTerminatedPolicyStatus()
        {
            Equal(PolicyStatus.Terminated, version.PolicyStatus);
            return this;
        }

        public PolicyVersionAssert BeDraft()
        {
            Equal(PolicyVersionStatus.Draft, version.VersionStatus);
            return this;
        }
        
        public PolicyVersionAssert BeActive()
        {
            Equal(PolicyVersionStatus.Active, version.VersionStatus);
            return this;
        }

        public PolicyVersionAssert HaveTotalPremiumEqualTo(Money amount)
        {
            Equal(amount, version.TotalPremium);
            return this;
        }
        
        public PolicyVersionAssert StartOn(DateTime theDate)
        {
            Equal(theDate, version.VersionPeriod.ValidFrom);
            return this;
        }
        
        public PolicyVersionAssert EndsOn(DateTime theDate)
        {
            Equal(theDate, version.VersionPeriod.ValidTo);
            return this;
        }

        public PolicyVersionAssert CoverPeriod(DateTime from, DateTime to)
        {
            Equal(from, version.CoverPeriod.ValidFrom);
            Equal(to, version.CoverPeriod.ValidTo);
            return this;
        }
    }

    public static class PolicyAssertsExtension
    {
        public static PolicyAssert Should(this Policy policy)
        {
            return new PolicyAssert(policy);
        }
    }
    
    public static class PolicyVersionAssertsExtension
    {
        public static PolicyVersionAssert Should(this PolicyVersion version)
        {
            return new PolicyVersionAssert(version);
        }
    }
}