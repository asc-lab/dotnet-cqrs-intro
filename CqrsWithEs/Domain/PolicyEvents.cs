using System;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;

namespace CqrsWithEs.Domain
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
            
        }
    }

    public class CoverageExtendedPolicyVersionConfirmed : Event
    {
    }

    public class CoverageExtendedPolicyVersionCancelled : Event
    {
    }

    //---------------------------------------------------------------


    public class PersonData
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string TaxId { get; private set; }

        public PersonData(string firstName, string lastName, string taxId)
        {
            FirstName = firstName;
            LastName = lastName;
            TaxId = taxId;
        }
    }

    public class CarData
    {
        public string Make { get; private set; }
        public string PlateNumber { get; private set; }
        public int ProductionYear { get; private set; }

        public CarData(string make, string plateNumber, int productionYear)
        {
            Make = make;
            PlateNumber = plateNumber;
            ProductionYear = productionYear;
        }
    }

    public class PolicyCoverData
    {
        public string Code { get; private set; }
        public DateTime CoverFrom { get; private set; }
        public DateTime CoverTo { get; private set; }
        public Money Amount { get; private set; }
        public Money Price { get; private set; }
        public TimeSpan PriceUnit { get; private set; }

        public PolicyCoverData(string code, DateTime coverFrom, DateTime coverTo, Money amount, Money price, TimeSpan priceUnit)
        {
            Code = code;
            CoverFrom = coverFrom;
            CoverTo = coverTo;
            Amount = amount;
            Price = price;
            PriceUnit = priceUnit;
        }
    }
}