using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NoCqrs.Domain
{
    public class Policy
    {
        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public Product Product { get; private set; }
        private List<PolicyVersion> versions = new List<PolicyVersion>();
        public IEnumerable<PolicyVersion> Versions => versions.AsReadOnly();
        public DateTime PurchaseDate { get; private set; }
        public PolicyVersion CurrentVersion { get; private set; }

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

        private bool Terminated()
        {
            return versions.Any(v => v.IsActive() && v.PolicyStatus == PolicyStatus.Terminated);
        }

        public void TerminatePolicy(DateTime effectiveDateOfChange)
        {
            if (Terminated())
            {
                throw new ApplicationException("Policy already terminated");
            } 
            
            var versionAtEffectiveDate = versions.EffectiveAtDate(effectiveDateOfChange);

            if (versionAtEffectiveDate == null)
            {
                throw new ApplicationException("No active version at given date");    
            }
            
            var termVer = AddNewVersionBasedOn
            (
                versionAtEffectiveDate, 
                effectiveDateOfChange
            );

            termVer.EndPolicyOn(effectiveDateOfChange.AddDays(-1));

        }

        public void CancelLastAnnex()
        {
            var lastActiveVer = versions.LatestActive();

            if (lastActiveVer.VersionNumber == 1)
            {
                throw new ApplicationException("There are no annexed left to cancel");
            }

            lastActiveVer.Cancel();

            //WARNING: Added to support queries 
            CurrentVersion = Versions.LatestActive();
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
            
            //WARNING: Added to support queries 
            CurrentVersion = Versions.LatestActive();
        }

        private void AddFirstVersion(Offer offer, DateTime purchaseDate, DateTime policyStartDate)
        {
            var ver = new PolicyVersion
            (
                Guid.NewGuid(),
                1,
                ValidityPeriod.Between(policyStartDate, policyStartDate.Add(offer.CoverPeriod)),
                PolicyStatus.Active,
                ValidityPeriod.Between(policyStartDate, policyStartDate.Add(offer.CoverPeriod)),
                offer.Customer.Copy(),
                offer.Driver.Copy(),
                offer.Car.Copy(),
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

    }

    public enum PolicyStatus
    {
        Active,
        Terminated
    }
}