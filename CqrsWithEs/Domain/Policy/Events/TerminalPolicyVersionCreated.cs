using System;
using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain.Base;

namespace CqrsWithEs.Domain.Policy.Events
{
    public class TerminalPolicyVersionCreated : Event
    {
        public int VersionNumber { get; private set; }
        public int BaseVersionNumber { get; private set; }
        public DateTime VersionFrom { get; private set; }
        public DateTime VersionTo { get; private set; }
        public DateTime CoverFrom { get; private set; }
        public DateTime CoverTo { get; private set; }
        public List<PolicyCoverData> Covers { get; private set; }

        public TerminalPolicyVersionCreated(
            int versionNumber, 
            int baseVersionNumber, 
            DateTime versionFrom, 
            DateTime versionTo, 
            DateTime coverFrom, 
            DateTime coverTo, 
            List<PolicyCoverData> covers)
        {
            VersionNumber = versionNumber;
            BaseVersionNumber = baseVersionNumber;
            VersionFrom = versionFrom;
            VersionTo = versionTo;
            CoverFrom = coverFrom;
            CoverTo = coverTo;
            Covers = covers;
        }
        
        public TerminalPolicyVersionCreated(
            int versionNumber, 
            int baseVersionNumber, 
            DateTime versionFrom, 
            DateTime versionTo, 
            DateTime coverFrom, 
            DateTime coverTo, 
            List<PolicyCover> covers)
        {
            VersionNumber = versionNumber;
            BaseVersionNumber = baseVersionNumber;
            VersionFrom = versionFrom;
            VersionTo = versionTo;
            CoverFrom = coverFrom;
            CoverTo = coverTo;
            Covers = covers
                .Select(c => new PolicyCoverData(c.CoverCode, c.CoverPeriod.ValidFrom, c.CoverPeriod.ValidTo, c.Amount,
                    c.Price.Price, c.Price.PricePeriod))
                .ToList();
        }
    }
}