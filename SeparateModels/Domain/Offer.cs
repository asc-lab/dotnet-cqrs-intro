using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NodaMoney;

namespace SeparateModels.Domain
{
    public class Offer
    {
        public Guid Id { get; private set; }
        public string Number { get; private set; }
        public OfferStatus Status { get; private set; }
        public string ProductCode { get; private set; }
        public Person Customer { get; private set; }
        public Car Car { get; private set; }
        public Person Driver { get; private set; }
        public TimeSpan CoverPeriod { get; private set; }
        public Money TotalCost { get; private set; }
        public DateTime CreationDate { get; private set; }
        public DateTime ValidityDate => CreationDate.AddDays(30);
        [JsonProperty]
        private List<CoverPrice> covers = new List<CoverPrice>();
        [JsonIgnore]
        public IEnumerable<CoverPrice> Covers => covers.AsReadOnly();

        public Offer
        (
            Guid id, 
            string number, 
            Product product, 
            Person customer, 
            Person driver, 
            Car car,
            TimeSpan coverPeriod, 
            Money totalCost, 
            DateTime creationDate,
            IDictionary<Cover, Money> coversPrices
        )
        {
            Id = id;
            Number = number;
            Status = OfferStatus.New;
            ProductCode = product.Code;
            Customer = customer;
            Driver = driver;
            Car = car;
            CoverPeriod = coverPeriod;
            TotalCost = totalCost;
            CreationDate = creationDate;
            foreach (var coverWithPrice in coversPrices)
            {
                covers.Add(new CoverPrice(Guid.NewGuid(), coverWithPrice.Key, coverWithPrice.Value, coverPeriod));
            }
        }

        //required by JSON
        public Offer()
        {
        }

        public bool Converted() => Status == OfferStatus.Converted;

        public bool Expired(DateTime theDate) => ValidityDate < theDate;

        public void Reject()
        {
            //intentionally oversimplified
            if (Status != OfferStatus.New)
            {
                throw new ApplicationException("Cannot reject offer in status other than New");
            }

            Status = OfferStatus.Rejected;
        }


        public void Convert()
        {
            //intentionally oversimplified
            if (Status != OfferStatus.New)
            {
                throw new ApplicationException("Cannot convert offer in status other than New");
            }

            Status = OfferStatus.Converted;
        }

        public bool Rejected() => Status == OfferStatus.Rejected;
    }

    public enum OfferStatus
    {
        New,
        Converted,
        Rejected
    }

    public class CoverPrice
    {
        public Guid Id { get; private set; }
        public string CoverCode { get; private set; }
        public Money Price { get; private set; }
        public TimeSpan CoverPeriod { get; private set; }

        public CoverPrice(Guid id, Cover cover, Money price, TimeSpan coverPeriod)
        {
            Id = id;
            CoverCode = cover.Code;
            Price = price;
            CoverPeriod = coverPeriod;
        }
        
        public CoverPrice(Cover cover, Money price, TimeSpan coverPeriod)
        {
            Id = Guid.NewGuid();
            CoverCode = cover.Code;
            Price = price;
            CoverPeriod = coverPeriod;
        }

        //required by EF
        protected CoverPrice()
        {
        } 
    }
}