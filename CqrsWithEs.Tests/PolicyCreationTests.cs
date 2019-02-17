using System;
using System.Collections.Generic;
using System.Linq;
using CqrsWithEs.Domain;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Policy.Events;
using CqrsWithEs.Tests.Asserts;
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
            
            var policy = Policy.BuyOffer(offer, purchaseDate, policyStartDate);

            var resultingEvents = policy.GetUncommittedChanges();
            
            //assert state
            policy
                .Should()
                .HaveVersions(1);
                
            policy.Versions.WithNumber(1)
                .Should()
                .BeActive()
                .HaveActivePolicyStatus()
                .HaveTotalPremiumEqualTo(Money.Euro(500));
            
            //assert events
            resultingEvents
                .Should()
                .BeSingle()
                .ContainEvent(
                    new InitialPolicyVersionCreated
                    (
                        policy.PolicyNumber,
                        "STD_CAR_INSURANCE",
                        policyStartDate,
                        policyStartDate.AddDays(365), 
                        purchaseDate,
                        new PersonData("Jan","Kowalski","1111111116"),
                        new CarData("Ford Focus","WAW1010",2005),
                        new List<PolicyCoverData>
                        {
                            new PolicyCoverData("OC", policyStartDate, policyStartDate.AddDays(365), Money.Euro(500),Money.Euro(500), TimeSpan.FromDays(365))
                        }
                    )
                );

        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItExpires()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 10));

            var ex = Throws<ApplicationException>(() => Policy.BuyOffer(offer, new DateTime(2019, 1, 12),
                new DateTime(2019, 1, 15)));
            
            Equal("Offer expired", ex.Message);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItsConverted()
        {
            var offer = OffersTestData.ConvertedOfferValidUntil(new DateTime(2019, 1, 10));
            
            var ex = Throws<ApplicationException>(() => Policy.BuyOffer(offer, new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 10)));
            
            Equal("Offer already converted", ex.Message);
        }
        
        [Fact]
        public void CannotConvertOfferToPolicyAfterItsRejected()
        {
            var offer = OffersTestData.RejectedOfferValidUntil(new DateTime(2019, 1, 10));
            
            var ex = Throws<ApplicationException>(() => Policy.BuyOffer(offer, new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 10)));
            
            Equal("Offer already rejected", ex.Message);
        }
        
        [Fact]
        public void CannotConvertStartPolicyAfterOfferExpiryDate()
        {
            var offer = OffersTestData.StandardOneYearOCOfferValidUntil(new DateTime(2019, 1, 10));

            var ex = Throws<ApplicationException>(() => Policy.BuyOffer(offer, new DateTime(2019, 1, 10),
                new DateTime(2019, 1, 15)));
            
            Equal("Offer not valid at policy start date", ex.Message);
        }
    }
}