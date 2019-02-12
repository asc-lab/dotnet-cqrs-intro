using System;
using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain.Base;
using CqrsWithEs.Domain.Common;
using CqrsWithEs.Domain.Offer;
using CqrsWithEs.Domain.Policy.Events;

namespace CqrsWithEs.Domain.Policy
{
    public class Policy : AggregateRoot
    {
        public string PolicyNumber { get; private set; }
        private List<PolicyVersion> versions = new List<PolicyVersion>();
        public IList<PolicyVersion> Versions => versions.AsReadOnly();

        public Policy(Guid id, IEnumerable<Event> events)
        {
            Id = id;
            LoadsFromHistory(events);
        }

        public static Policy BuyOffer(Offer.Offer offer, DateTime purchaseDate, DateTime policyStartDate)
        {
            return new Policy(offer, purchaseDate, policyStartDate);
        }
        
        private Policy(Offer.Offer offer, DateTime purchaseDate, DateTime policyStartDate)
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

        public void ConfirmCoverageExtension()
        {
            var lastVersion = versions.Last();

            if (lastVersion.VersionStatus != PolicyVersionStatus.Draft)
            {
                throw new ApplicationException("There is no extension of coverage pending");
            }
            
            if (lastVersion.PolicyStatus != PolicyStatus.Active)
            {
                throw new ApplicationException("Pending version is not coverage extension");
            }
            
            ApplyChange(new CoverageExtendedPolicyVersionConfirmed(lastVersion.VersionNumber));
        }

        private void Apply(CoverageExtendedPolicyVersionConfirmed @event)
        {
            var versionToConfirm = versions.WithNumber(@event.VersionNumber);
            versionToConfirm.Confirm();
        }

        public void TerminatePolicy(DateTime effectiveDateOfChange)
        {
            if (Terminated())
            {
                throw new ApplicationException("Policy already terminated");
            } 
            
            var versionAtDate = versions.EffectiveAt(effectiveDateOfChange);

            if (versionAtDate == null)
            {
                throw new ApplicationException("No active version at given date");    
            }

            if (!versionAtDate.CoversDate(effectiveDateOfChange))
            {
                throw new ApplicationException("Cannot terminate policy at given date as it is not withing cover period");    
            }
            
            var newVersionNumber = versions.Count + 1;
            var versionPeriod = ValidityPeriod.Between(effectiveDateOfChange, versionAtDate.CoverPeriod.ValidTo);
            var coverPeriod = versionAtDate.CoverPeriod.EndOn(effectiveDateOfChange.AddDays(-1));
            var terminatedCovers = versionAtDate.Covers
                .Select(c => c.EndOn(effectiveDateOfChange.AddDays(-1)))
                .ToList();

            ApplyChange
            (
                new TerminalPolicyVersionCreated
                (
                    newVersionNumber,
                    versionAtDate.VersionNumber,
                    versionPeriod.ValidFrom,
                    versionPeriod.ValidTo,
                    coverPeriod.ValidFrom,
                    coverPeriod.ValidTo,
                    terminatedCovers
                )
            );
        }

        private void Apply(TerminalPolicyVersionCreated @event)
        {
            var versionPeriod = ValidityPeriod.Between(@event.VersionFrom, @event.VersionTo);
            
            var draft = new PolicyVersion
            (
                @event.VersionNumber,
                PolicyStatus.Terminated,
                PolicyVersionStatus.Draft,
                ValidityPeriod.Between(@event.CoverFrom,@event.CoverTo),
                versionPeriod,
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
            );
            
            versions.Add(draft);
        }

        public void ConfirmTermination()
        {
            var lastVersion = versions.Last();

            if (lastVersion.VersionStatus != PolicyVersionStatus.Draft)
            {
                throw new ApplicationException("There is no termination pending");
            }
            
            if (lastVersion.PolicyStatus != PolicyStatus.Terminated)
            {
                throw new ApplicationException("Pending version is not terminal");
            }
            
            ApplyChange(new TerminalPolicyVersionConfirmed(lastVersion.VersionNumber));
        }

        private void Apply(TerminalPolicyVersionConfirmed @event)
        {
            var versionToConfirm = versions.WithNumber(@event.VersionNumber);
            versionToConfirm.Confirm();
        }

        public IPolicyState EffectiveStateAt(DateTime theDate)
        {
            return versions.EffectiveAt(theDate);
        }

        private bool Terminated() => versions.Any(v =>
            v.VersionStatus == PolicyVersionStatus.Active && v.PolicyStatus == PolicyStatus.Terminated);
    }
}