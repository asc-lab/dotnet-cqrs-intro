using System;
using CqrsWithEs.Domain.Base;
using CqrsWithEs.Domain.Common;

namespace CqrsWithEs.Domain.Policy.Events
{
    public class CoverageExtendedPolicyVersionCreated : Event
    {
        public int VersionNumber { get; private set; }
        public int BaseVersionNumber { get; private set; }
        public DateTime VersionFrom { get; private set; }
        public DateTime VersionTo { get; private set; }
        public PolicyCoverData NewCover { get; private set; }
        
        public CoverageExtendedPolicyVersionCreated
        (
            int versionNumber,
            int baseVersionNumber,
            ValidityPeriod versionPeriod,
            PolicyCover newCover
        )
        {
            VersionNumber = versionNumber;
            BaseVersionNumber = baseVersionNumber;
            VersionFrom = versionPeriod.ValidFrom;
            VersionTo = versionPeriod.ValidTo;
            NewCover = new PolicyCoverData
            (
                newCover.CoverCode,
                newCover.CoverPeriod.ValidFrom,
                newCover.CoverPeriod.ValidTo,
                newCover.Amount,
                newCover.Price.Price,
                newCover.Price.PricePeriod
            );
        }
        
        public CoverageExtendedPolicyVersionCreated
        (
            int versionNumber,
            int baseVersionNumber,
            ValidityPeriod versionPeriod,
            PolicyCoverData newCover
        )
        {
            VersionNumber = versionNumber;
            BaseVersionNumber = baseVersionNumber;
            VersionFrom = versionPeriod.ValidFrom;
            VersionTo = versionPeriod.ValidTo;
            NewCover = newCover;
        }
    }
}