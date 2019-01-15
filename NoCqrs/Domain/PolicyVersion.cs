using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;

namespace NoCqrs.Domain
{
    public class PolicyVersion
    {
        public Guid Id { get; private set; }
        public int VersionNumber { get; private set; }
        public PolicyVersionStatus Status { get; private set; }
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
            Status = PolicyVersionStatus.Draft;
            VersionValidityPeriod = versionValidityPeriod;
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
            VersionValidityPeriod = ValidityPeriod.Between(startDate, baseVersion.CoverPeriod.ValidTo);
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

            Status = PolicyVersionStatus.Active;
        }

        public bool IsDraft() => Status == PolicyVersionStatus.Draft;
    }

    public enum PolicyVersionStatus
    {
        Draft,
        Active,
        Cancelled
    }
}