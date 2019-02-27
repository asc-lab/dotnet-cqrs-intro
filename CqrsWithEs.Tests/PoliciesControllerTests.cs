using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsWithEs.Commands;
using CqrsWithEs.Tests.HttpClientAsserts;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CqrsWithEs.Tests
{
    public class PoliciesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public PoliciesControllerTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task CanCreatePolicyFromValidNotExpiredOffer()
        {
            var client = factory.CreateClient();

            var response = await client.DoPostAsync<CreatePolicyResult>("/api/Policies", new CreatePolicyCommand
            {
                OfferNumber = "OFF001",
                PurchaseDate = new DateTime(2019,2,26),
                PolicyStartDate = new DateTime(2019,2,27)
            });

            Assert.True(response.Success);
            Assert.NotEmpty(response.Data.PolicyNumber);
        }

        [Fact]
        public async Task CanBuyAdditionalCover()
        {
            var client = factory.CreateClient();

            var createPolicyResponse = await client.DoPostAsync<CreatePolicyResult>("/api/Policies", new CreatePolicyCommand
            {
                OfferNumber = "OFF001",
                PurchaseDate = new DateTime(2019,2,26),
                PolicyStartDate = new DateTime(2019,2,27)
            });


            var buyCoverResponse = await client.DoPostAsync<BuyAdditionalCoverResult>("/api/Policies/BuyAdditionalCover", new BuyAdditionalCoverCommand
            {
               PolicyId = createPolicyResponse.Data.PolicyId,
                NewCoverCode = "AC",
                NewCoverPrice = 100M,
                NewCoverPriceUnit = TimeSpan.FromDays(365),
                EffectiveDateOfChange = new DateTime(2019,6,1)
            });
            
            Assert.Equal(2, buyCoverResponse.Data.VersionWithAdditionalCoversNumber);
        }
        
        [Fact]
        public async Task CanConfirmBuyAdditionalCover()
        {
            var client = factory.CreateClient();

            var createPolicyResponse = await client.DoPostAsync<CreatePolicyResult>("/api/Policies", new CreatePolicyCommand
            {
                OfferNumber = "OFF001",
                PurchaseDate = new DateTime(2019,2,26),
                PolicyStartDate = new DateTime(2019,2,27)
            });


            var buyCoverResponse = await client.DoPostAsync<BuyAdditionalCoverResult>("/api/Policies/BuyAdditionalCover", new BuyAdditionalCoverCommand
            {
                PolicyId = createPolicyResponse.Data.PolicyId,
                NewCoverCode = "AC",
                NewCoverPrice = 100M,
                NewCoverPriceUnit = TimeSpan.FromDays(365),
                EffectiveDateOfChange = new DateTime(2019,6,1)
            });
            
            var confirmBuyCoverResponse = await client.DoPostAsync<ConfirmBuyAdditionalCoverResult>("/api/Policies/ConfirmBuyAdditionalCover", new ConfirmBuyAdditionalCoverCommand
            {
                PolicyId = createPolicyResponse.Data.PolicyId,
                VersionToConfirmNumber = buyCoverResponse.Data.VersionWithAdditionalCoversNumber
            });
            
            Assert.Equal(2, confirmBuyCoverResponse.Data.VersionConfirmed);
        }

        [Fact]
        public async Task CanTerminatePolicy()
        {
            var client = factory.CreateClient();

            var createPolicyResponse = await client.DoPostAsync<CreatePolicyResult>("/api/Policies", new CreatePolicyCommand
            {
                OfferNumber = "OFF001",
                PurchaseDate = new DateTime(2019,2,26),
                PolicyStartDate = new DateTime(2019,2,27)
            });


            var terminateResponse = await client.DoPostAsync<TerminatePolicyResult>("/api/Policies/Terminate", new TerminatePolicyCommand
            {
                PolicyId = createPolicyResponse.Data.PolicyId,
                TerminationDate = new DateTime(2019,6,1)
            });
            
            Assert.Equal(2, terminateResponse.Data.VersionWithTerminationNumber);
        }
        
        [Fact]
        public async Task CanConfirmTerminationPolicy()
        {
            var client = factory.CreateClient();

            var createPolicyResponse = await client.DoPostAsync<CreatePolicyResult>("/api/Policies", new CreatePolicyCommand
            {
                OfferNumber = "OFF001",
                PurchaseDate = new DateTime(2019,2,26),
                PolicyStartDate = new DateTime(2019,2,27)
            });


            var terminateResponse = await client.DoPostAsync<TerminatePolicyResult>("/api/Policies/Terminate", new TerminatePolicyCommand
            {
                PolicyId = createPolicyResponse.Data.PolicyId,
                TerminationDate = new DateTime(2019,6,1)
            });
            
            var confirmTerminateResponse = await client.DoPostAsync<ConfirmTerminationResult>("/api/Policies/ConfirmTermination", new ConfirmTerminationCommand
            {
                PolicyId = createPolicyResponse.Data.PolicyId,
                VersionToConfirmNumber = 2
            });
            
            
            Assert.Equal(2, confirmTerminateResponse.Data.VersionConfirmed);
        }
    }
}