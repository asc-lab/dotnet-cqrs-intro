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

    }
}