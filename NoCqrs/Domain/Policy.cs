using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NoCqrs.Domain
{
    public class Policy
    {
        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public Product Product { get; private set; }
        public PolicyStatus Status { get; private set; }
        private List<PolicyVersion> versions = new List<PolicyVersion>();
        public IReadOnlyCollection<PolicyVersion> Versions => new ReadOnlyCollection<PolicyVersion>(versions);
        public DateTime PurchaseDate { get; private set; }

        public static Policy ConvertOffer(Offer offer, string PolicyNumber, DateTime purchaseDate, DateTime policyStartDate)
        {
            //preconditions
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

            //state changes
            var newPolicy = new Policy
            {
                Id = Guid.NewGuid(),
                Number = PolicyNumber,
                Product = offer.Product,
                Status = PolicyStatus.Active,
                PurchaseDate = purchaseDate
            };

            newPolicy.AddFirstVersion(offer, purchaseDate, policyStartDate);
            
            newPolicy.ConfirmChanges(1);

            return newPolicy;
        }
        
        public void ExtendCoverage(DateTime effectiveDateOfChange, CoverPrice newCover)
        {
            //preconditions
            if (Terminated())
            {
                throw new ApplicationException("Cannot annex terminated policy");
            }

            
            var versionAtEffectiveDate = versions.EffectiveAtDate(effectiveDateOfChange);

            if (versionAtEffectiveDate == null)
            {
                throw new ApplicationException("No active version at given date");    
            }

            //create new version starting since effectiveDateOfChange based on versionAtEffectiveDate
            var annexVer = AddNewVersionBasedOn
            (
                versionAtEffectiveDate, 
                effectiveDateOfChange
            );
            
            //add new cover
            annexVer.AddCover(newCover, effectiveDateOfChange, annexVer.CoverPeriod.ValidTo);

        }
        
        public void TerminatePolicy(DateTime effectiveDateOfChange)
        {
            
        }

        public void ConfirmChanges(int versionToConfirmNumber)
        {
            //ensure version with number is draft and there are no drafts 
            var versionToConfirm = versions.WithNumber(versionToConfirmNumber);

            if (versionToConfirm == null)
            {
                throw new ApplicationException("Version not found");
            }

            versionToConfirm.Confirm();
        }

        private void AddFirstVersion(Offer offer, DateTime purchaseDate, DateTime policyStartDate)
        {
            var ver = new PolicyVersion
            (
                Guid.NewGuid(),
                1,
                ValidityPeriod.Between(policyStartDate, policyStartDate.Add(offer.CoverPeriod)),
                ValidityPeriod.Between(policyStartDate, policyStartDate.Add(offer.CoverPeriod)),
                offer.Customer,
                offer.Driver,
                offer.Car,
                offer.TotalCost,
                offer.Covers
            );
            
            versions.Add(ver);
        }

        

        private PolicyVersion AddNewVersionBasedOn(PolicyVersion versionAtEffectiveDate, DateTime effectiveDateOfChange)
        {
            var newVersion = new PolicyVersion
            (
                versionAtEffectiveDate,
                versions.MaxVersionNumber() + 1,
                effectiveDateOfChange
            );
            versions.Add(newVersion);
            return newVersion;
        }

        private bool Terminated() => Status == PolicyStatus.Terminated;

    }

    public enum PolicyStatus
    {
        Active,
        Terminated
    }
}