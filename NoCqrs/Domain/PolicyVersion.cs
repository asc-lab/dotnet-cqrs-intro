using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Internal;
using NodaMoney;

namespace NoCqrs.Domain
{
    public class PolicyVersion
    {
        public Guid Id { get; private set; }
        public int VersionNumber { get; private set; }
        public int? BaseVersionNumber { get; private set; }
        public PolicyVersionStatus VersionStatus { get; private set; }
        public PolicyStatus PolicyStatus { get; private set; }
        public ValidityPeriod VersionValidityPeriod { get; private set; }
        public ValidityPeriod CoverPeriod { get; private set; }
        public Person PolicyHolder { get; private set; }
        public Person Driver { get; private set; }
        public Car Car { get; private set; }
        public Money TotalPremium { get; private set; }
        private List<PolicyCover> covers = new List<PolicyCover>();
        public IEnumerable<PolicyCover> Covers => covers.AsReadOnly();

        public PolicyVersion
        (
            Guid id,
            int versionNumber,
            ValidityPeriod versionValidityPeriod,
            PolicyStatus policyStatus,
            ValidityPeriod coverPeriod,
            Person policyHolder,
            Person driver,
            Car car,
            Money totalPremium,
            IEnumerable<CoverPrice> coverPrices
        )
        {
            Id = id;
            VersionNumber = versionNumber;
            BaseVersionNumber = null;
            VersionStatus = PolicyVersionStatus.Draft;
            VersionValidityPeriod = versionValidityPeriod;
            PolicyStatus = policyStatus;
            CoverPeriod = coverPeriod;
            PolicyHolder = policyHolder;
            Driver = driver;
            Car = car;
            TotalPremium = totalPremium; //TODO: check against covers?
            foreach (var coverPrice in coverPrices)
            {
                AddCover(coverPrice, versionValidityPeriod.ValidFrom, versionValidityPeriod.ValidTo);
            }
        }


        public PolicyVersion
        (
            PolicyVersion baseVersion,
            int versionNumber,
            DateTime startDate
        )
        {
            Id = Guid.NewGuid();
            VersionNumber = versionNumber;
            BaseVersionNumber = baseVersion.VersionNumber;
            VersionValidityPeriod = ValidityPeriod.Between(startDate, baseVersion.CoverPeriod.ValidTo);
            PolicyStatus = baseVersion.PolicyStatus;
            CoverPeriod = ValidityPeriod.Between(baseVersion.CoverPeriod.ValidFrom, baseVersion.CoverPeriod.ValidTo);
            PolicyHolder = baseVersion.PolicyHolder.Copy();
            Driver = baseVersion.Driver.Copy();
            Car = baseVersion.Car.Copy();
            foreach (var oldCover in baseVersion.covers)
            {
                covers.Add(oldCover);
            }

            TotalPremium = RecalculateTotal();
        }

        // required by EF
        protected PolicyVersion()
        {
        }

        public void AddCover(CoverPrice coverPrice, DateTime coverStart, DateTime coverEnd)
        {
            if (!IsDraft())
            {
                throw new ApplicationException("Only draft versions can be altered");
            }

            var cover = new PolicyCover
            (
                Guid.NewGuid(),
                coverPrice.Cover,
                ValidityPeriod.Between(coverStart, coverEnd),
                coverPrice.Price,
                coverPrice.CoverPeriod
            );
            covers.Add(cover);

            TotalPremium = RecalculateTotal();
        }

        private Money RecalculateTotal() => covers.Aggregate(Money.Euro(0M), (c, x) => c + x.Amount);

        public bool IsEffectiveOn(DateTime effectiveDate) => VersionValidityPeriod.Contains(effectiveDate);

        public void Confirm()
        {
            if (!IsDraft())
            {
                throw new ApplicationException("Only draft can be confirmed");
            }

            VersionStatus = PolicyVersionStatus.Active;
        }

        public bool IsDraft() => VersionStatus == PolicyVersionStatus.Draft;

        public bool IsActive() => VersionStatus == PolicyVersionStatus.Active;

        public void EndPolicyOn(DateTime lastDayOfCover)
        {
            if (!IsDraft())
            {
                throw new ApplicationException("Only draft versions can be altered");
            }

            CoverPeriod = CoverPeriod.EndOn(lastDayOfCover);

            foreach (var cover in covers)
            {
                cover.EndCoverOn(lastDayOfCover);
            }
            
            TotalPremium = RecalculateTotal();

            PolicyStatus = PolicyStatus.Terminated;
        }

        public void Cancel()
        {
            if (!IsActive())
            {
                throw new ApplicationException("Only active version can be cancelled");
            }

            VersionStatus = PolicyVersionStatus.Cancelled;
        }
    }

    public enum PolicyVersionStatus
    {
        Draft,
        Active,
        Cancelled
    }
}