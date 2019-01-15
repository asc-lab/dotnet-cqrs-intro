using System;
using System.Linq;
using NoCqrs.Domain;
using NodaMoney;
using Xunit;
using static Xunit.Assert;

namespace NoCqrs.Tests
{
    public class PolicyCreationUnitTest
    {
        [Fact]
        public void CanConvertOfferToPolicyBeforeItExpires()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 31));

            var policy = Policy.ConvertOffer(offer,"POL0001", new DateTime(2019,1,12), new DateTime(2019,1,15));
            
            NotNull(policy);
            Equal(PolicyStatus.Active, policy.Status);
            Single(policy.Versions);
            Equal(PolicyVersionStatus.Active, policy.Versions.WithNumber(1).Status);
            Equal(Money.Euro(500), policy.Versions.WithNumber(1).TotalPremium);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItExpires()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 10));

            var ex = Throws<ApplicationException>(() => Policy.ConvertOffer(offer, "POL0001", new DateTime(2019, 1, 12),
                new DateTime(2019, 1, 15)));
            
            Equal("Offer expired", ex.Message);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItsConverted()
        {
            var offer = OffersTestData.ConvertedOfferValidUntil(new DateTime(2019, 1, 10));
            
            var ex = Throws<ApplicationException>(() => Policy.ConvertOffer(offer, "POL0001", new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 10)));
            
            Equal("Offer already converted", ex.Message);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItsRejected()
        {
            var offer = OffersTestData.RejectedOfferValidUntil(new DateTime(2019, 1, 10));
            
            var ex = Throws<ApplicationException>(() => Policy.ConvertOffer(offer, "POL0001", new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 10)));
            
            Equal("Offer already rejected", ex.Message);
        }
        
        [Fact]
        public void CannotConvertStartPolicyAfterOfferExpiryDate()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 10));

            var ex = Throws<ApplicationException>(() => Policy.ConvertOffer(offer, "POL0001", new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 15)));
            
            Equal("Offer not valid at policy start date", ex.Message);
        }
        
    }
}