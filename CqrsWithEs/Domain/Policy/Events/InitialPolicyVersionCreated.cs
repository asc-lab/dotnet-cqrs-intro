using System;
using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain.Base;
using CqrsWithEs.Domain.Common;

namespace CqrsWithEs.Domain.Policy.Events
{
    public class InitialPolicyVersionCreated : Event
    {
        public string PolicyNumber { get; private set; }
        public PolicyStatus PolicyStatus { get; private set; }
        public string ProductCode { get; private set; }
        public DateTime CoverFrom { get; private set; }
        public DateTime CoverTo{ get; private set; }
        public DateTime PurchaseDate { get; private set; }
        public PersonData PolicyHolder { get; private set; }
        public CarData Car { get; private set; }
        public List<PolicyCoverData> Covers { get; private set; }

        public InitialPolicyVersionCreated(
            string policyNumber, 
            string productCode, 
            DateTime coverFrom, 
            DateTime coverTo, 
            DateTime purchaseDate, 
            PersonData policyHolder, 
            CarData car, 
            List<PolicyCoverData> covers)
        {
            PolicyNumber = policyNumber;
            PolicyStatus = PolicyStatus.Active;
            ProductCode = productCode;
            CoverFrom = coverFrom;
            CoverTo = coverTo;
            PurchaseDate = purchaseDate;
            PolicyHolder = policyHolder;
            Car = car;
            Covers = covers;
        }
        
        public InitialPolicyVersionCreated(
            string policyNumber, 
            string productCode, 
            ValidityPeriod coverPeriod, 
            DateTime purchaseDate, 
            Person policyHolder, 
            Car car, 
            IEnumerable<PolicyCover> covers)
        {
            PolicyNumber = policyNumber;
            PolicyStatus = PolicyStatus.Active;
            ProductCode = productCode;
            CoverFrom = coverPeriod.ValidFrom;
            CoverTo = coverPeriod.ValidTo;
            PurchaseDate = purchaseDate;
            PolicyHolder = new PersonData(policyHolder.FirstName, policyHolder.LastName, policyHolder.TaxId);
            Car = new CarData(car.Make,car.PlateNumber,car.ProductionYear);
            Covers = covers
                .Select(c => new PolicyCoverData
                    (
                        c.CoverCode,
                        c.CoverPeriod.ValidFrom,
                        c.CoverPeriod.ValidTo,
                        c.Amount,
                        c.Price.Price,
                        c.Price.PricePeriod
                    )
                )
                .ToList();
        }
    }
}