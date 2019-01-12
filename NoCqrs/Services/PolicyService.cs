using System;
using NoCqrs.Domain;

namespace NoCqrs.Services
{
    public class PolicyService
    {
        private readonly IDataStore dataStore;

        public PolicyService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public CreatePolicyResult CreatePolicy(CreatePolicyRequest request)
        {
            var offer = dataStore.Offers.WithNumber(request.OfferNumber);
            var policy = Policy.ConvertOffer(offer, Guid.NewGuid().ToString(), request.PurchaseDate, request.PolicyStartDate);
            dataStore.Policies.Add(policy);
            dataStore.CommitChanges();
            return new CreatePolicyResult
            {
                PolicyNumber = policy.Number
            };
        }
    }
}