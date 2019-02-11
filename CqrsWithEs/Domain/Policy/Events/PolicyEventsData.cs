using System;
using NodaMoney;

namespace CqrsWithEs.Domain.Policy.Events
{

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