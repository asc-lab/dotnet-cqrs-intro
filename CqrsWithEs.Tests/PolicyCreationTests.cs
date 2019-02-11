using System;
using System.Linq;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Policy.Events;
using NodaMoney;
using Xunit;
using static Xunit.Assert;

namespace CqrsWithEs.Tests
{
    public class PolicyCreationTests
    {
        [Fact]
        public void CanConvertOfferToPolicyBeforeItExpires()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 31));
            var purchaseDate = new DateTime(2019, 1, 12);
            var policyStartDate = new DateTime(2019, 1, 15);
            
            var policy = new Policy(offer, purchaseDate, policyStartDate);

            var resultingEvents = policy.GetUncommittedChanges();
            
            //assert state
            NotNull(policy);
            Single(policy.Versions);
            Equal(PolicyStatus.Active, policy.Versions.WithNumber(1).PolicyStatus);
            Equal(PolicyVersionStatus.Active, policy.Versions.WithNumber(1).VersionStatus);
            Equal(Money.Euro(500), policy.Versions.WithNumber(1).TotalPremium);
            
            //assert events
            Single(resultingEvents);
            IsType<InitialPolicyVersionCreated>(resultingEvents.First());

        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItExpires()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 10));

            var ex = Throws<ApplicationException>(() => new Policy(offer, new DateTime(2019, 1, 12),
                new DateTime(2019, 1, 15)));
            
            Equal("Offer expired", ex.Message);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItsConverted()
        {
            var offer = OffersTestData.ConvertedOfferValidUntil(new DateTime(2019, 1, 10));
            
            var ex = Throws<ApplicationException>(() => new Policy(offer, new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 10)));
            
            Equal("Offer already converted", ex.Message);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItsRejected()
        {
            var offer = OffersTestData.RejectedOfferValidUntil(new DateTime(2019, 1, 10));
            
            var ex = Throws<ApplicationException>(() => new Policy(offer, new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 10)));
            
            Equal("Offer already rejected", ex.Message);
        }
        
        [Fact]
        public void CannotConvertStartPolicyAfterOfferExpiryDate()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 10));

            var ex = Throws<ApplicationException>(() => new Policy(offer, new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 15)));
            
            Equal("Offer not valid at policy start date", ex.Message);
        }
    }
}