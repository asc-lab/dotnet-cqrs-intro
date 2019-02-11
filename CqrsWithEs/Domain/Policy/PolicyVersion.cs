using System;
using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain.Common;
using NodaMoney;

namespace CqrsWithEs.Domain.Policy
{
    public class PolicyVersion
    {
        public int VersionNumber { get; }
        public PolicyStatus PolicyStatus { get; }
        public PolicyVersionStatus VersionStatus { get; private set; }
        public ValidityPeriod CoverPeriod { get; }
        public ValidityPeriod VersionPeriod { get; }
        private List<PolicyCover> covers = new List<PolicyCover>();
        public IReadOnlyCollection<PolicyCover> Covers => covers.AsReadOnly();
        public Money TotalPremium => covers.Aggregate(Money.Euro(0), (sum, c) => sum + c.Amount);

        public PolicyVersion(
            int versionNumber, 
            PolicyStatus policyStatus, 
            PolicyVersionStatus versionStatus, 
            ValidityPeriod coverPeriod, 
            ValidityPeriod versionPeriod,
            IEnumerable<PolicyCover> policyCovers)
        {
            VersionNumber = versionNumber;
            PolicyStatus = policyStatus;
            VersionStatus = versionStatus;
            CoverPeriod = coverPeriod;
            VersionPeriod = versionPeriod;
            covers.AddRange(policyCovers);
        }

        public bool IsEffectiveOn(DateTime theDate) => VersionPeriod.Contains(theDate);

        public bool CoversDate(DateTime theDate) => CoverPeriod.Contains(theDate);

        public PolicyVersion CreateDraftCopy(int newVersionNumber, ValidityPeriod versionPeriod)
        {
            return new PolicyVersion
            (
                newVersionNumber,
                PolicyStatus.Active,
                PolicyVersionStatus.Draft,
                CoverPeriod,
                versionPeriod,
                new List<PolicyCover>(covers)
            );
        }

        public void AddCover(string coverCode, UnitPrice price, ValidityPeriod coverPeriod)
        {
            if (VersionStatus != PolicyVersionStatus.Draft)
            {
                throw new ApplicationException("Cannot modify non draft version");
            }
            
            //check if not already present??
            
            //TODO: check dates
            
            this.covers.Add(new PolicyCover(coverCode,coverPeriod,price));
        }

        public bool ContainsCover(string coverCode) => covers.Any(c => c.CoverCode == coverCode);

        public void Confirm()
        {
            if (VersionStatus != PolicyVersionStatus.Draft)
            {
                throw new ApplicationException("Only draft can be confirmed");
            }

            this.VersionStatus = PolicyVersionStatus.Active;
        }
    }
}