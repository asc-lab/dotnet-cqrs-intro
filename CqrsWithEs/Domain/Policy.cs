using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Internal;
using NodaMoney;

namespace CqrsWithEs.Domain
{
    public class Policy : AggregateRoot
    {
        public string PolicyNumber { get; private set; }
        public  PolicyStatus PolicyStatus { get; private set; }
        private List<PolicyVersion> versions = new List<PolicyVersion>();
        public IList<PolicyVersion> Versions => versions.AsReadOnly();

        public Policy(Guid id, IEnumerable<Event> events)
        {
            Id = id;
            LoadsFromHistory(events);
        }

        public Policy(Offer offer, DateTime purchaseDate, DateTime policyStartDate)
        {
            if (offer.Converted())
            {
                throw new ApplicationException("Offer already converted");    
            }
            
            if (offer.Rejected())
            {
                throw new ApplicationException("Offer already rejected");    
            }

            if (offer.Expired(purchaseDate))
            {
                throw new ApplicationException("Offer expired");
            }


            if (offer.Expired(policyStartDate))
            {
                throw new ApplicationException("Offer not valid at policy start date");
            }

            var policyNumber = Guid.NewGuid().ToString();
            var coverPeriod = ValidityPeriod.Between
            (
                policyStartDate,
                policyStartDate.Add(offer.CoverPeriod)
            );
            var covers = offer.Covers
                .Select(c => PolicyCover.ForPrice(c, coverPeriod))
                .ToList();
            
            ApplyChange(
                new InitialPolicyVersionCreated
                (
                    policyNumber,
                    offer.ProductCode,
                    coverPeriod,
                    purchaseDate,
                    offer.Customer,
                    offer.Car,
                    covers
                )
            );
        }

        private void Apply(InitialPolicyVersionCreated @event)
        {
            PolicyNumber = @event.PolicyNumber;
            PolicyStatus = @event.PolicyStatus;
            versions.Add(
                new PolicyVersion
                (
                    1,
                    PolicyStatus.Active,
                    PolicyVersionStatus.Active,
                    ValidityPeriod.Between(@event.CoverFrom, @event.CoverTo),
                    ValidityPeriod.Between(@event.CoverFrom, @event.CoverTo),
                    @event.Covers
                        .Select(c => 
                            new PolicyCover
                            (
                                c.Code,
                                ValidityPeriod.Between(c.CoverFrom,c.CoverTo),
                                new UnitPrice(c.Price,c.PriceUnit)
                            )
                        )
                        .ToList()
                )
            );

        }

        public void ExtendCoverage(DateTime effectiveDateOfChange, CoverPrice newCover)
        {
            if (Terminated())
            {
                throw new ApplicationException("Cannot annex terminated policy");
            }

            var versionAtDate = versions.EffectiveAt(effectiveDateOfChange);

            if (versionAtDate==null || !versionAtDate.CoversDate(effectiveDateOfChange))
            {
                throw new ApplicationException("Policy does not cover annex date");   
            }

            if (versionAtDate.ContainsCover(newCover.CoverCode))
            {
                throw new ApplicationException("This cover is already present");
            }

            var newVersionNumber = versions.Count + 1;
            var versionPeriod = ValidityPeriod.Between(effectiveDateOfChange, versionAtDate.CoverPeriod.ValidTo);
            var newCoverage = PolicyCover.ForPrice(newCover, versionPeriod);
            
            ApplyChange
            (
                new CoverageExtendedPolicyVersionCreated
                (
                    newVersionNumber,
                    versionAtDate.VersionNumber,
                    versionPeriod,
                    newCoverage
                )
            );
        }
        
        private void Apply(CoverageExtendedPolicyVersionCreated @event)
        {
            var versionPeriod = ValidityPeriod.Between(@event.VersionFrom, @event.VersionTo);
            var draft = versions.WithNumber(@event.BaseVersionNumber)
                .CreateDraftCopy(@event.VersionNumber, versionPeriod);
            draft.AddCover
            (
                @event.NewCover.Code, 
                new UnitPrice(@event.NewCover.Price, @event.NewCover.PriceUnit),
                ValidityPeriod.Between(@event.NewCover.CoverFrom, @event.NewCover.CoverTo)
            );
            versions.Add(draft);
        }

        private bool Terminated() => PolicyStatus == PolicyStatus.Terminated;
    }


    public enum PolicyStatus
    {
        Active,
        Terminated
    }

    public class PolicyVersion
    {
        public int VersionNumber { get; }
        public PolicyStatus PolicyStatus { get; }
        public PolicyVersionStatus PolicyVersionStatus { get; }
        public ValidityPeriod CoverPeriod { get; }
        public ValidityPeriod VersionPeriod { get; }
        private List<PolicyCover> covers = new List<PolicyCover>();
        public IReadOnlyCollection<PolicyCover> Covers => covers.AsReadOnly();
        public Money TotalPremium => covers.Aggregate(Money.Euro(0), (sum, c) => sum + c.Amount);

        public PolicyVersion(
            int versionNumber, 
            PolicyStatus policyStatus, 
            PolicyVersionStatus policyVersionStatus, 
            ValidityPeriod coverPeriod, 
            ValidityPeriod versionPeriod,
            IEnumerable<PolicyCover> policyCovers)
        {
            VersionNumber = versionNumber;
            PolicyStatus = policyStatus;
            PolicyVersionStatus = policyVersionStatus;
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
            if (PolicyVersionStatus != PolicyVersionStatus.Draft)
            {
                throw new ApplicationException("Cannot modify non draft version");
            }
            
            //TODO: check dates
            
            this.covers.Add(new PolicyCover(coverCode,coverPeriod,price));
        }

        public bool ContainsCover(string coverCode) => covers.Any(c => c.CoverCode == coverCode);
    }
    
    public enum PolicyVersionStatus
    {
        Draft,
        Active,
        Cancelled
    }

    public class PolicyCover
    {
        public string CoverCode { get; }
        public ValidityPeriod CoverPeriod { get; }
        public UnitPrice Price { get; }
        public Money Amount { get; }

        public PolicyCover(string coverCode, ValidityPeriod coverPeriod, UnitPrice price)
        {
            CoverCode = coverCode;
            CoverPeriod = coverPeriod;
            Price = price;
            Amount = CalculateAmount();
        }

        public static PolicyCover ForPrice(CoverPrice coverPrice, ValidityPeriod coverPeriod)
        {
            return new PolicyCover
            (
                coverPrice.CoverCode,
                coverPeriod,
                new UnitPrice(coverPrice.Price, coverPrice.CoverPeriod)
            );
        }
        
        private Money CalculateAmount()
        {
            return decimal.Divide(CoverPeriod.Days, Price.PricePeriod.Days) * Price.Price;
        }
    }
}