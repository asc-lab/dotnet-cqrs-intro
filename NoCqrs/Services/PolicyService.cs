using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<PolicyInfoDto> SearchPolicies(SearchPolicyRequest request)
        {
            var policyFilter = new PolicyFilter
            (
                request.PolicyNumber,
                request.PolicyHolderFirstName,
                request.PolicyHolderLastName,
                request.PolicyStartFrom,
                request.PolicyStartTo,
                request.CarPlateNumber
            );

            var results = dataStore.Policies.Find(policyFilter);

            return results
                .Select(p => PolicyInfoDtoAssembler.AssemblePolicyInfoDto(p, p.CurrentVersion))
                .ToList();
        }

        public PolicyDto GetPolicyDetails(string policyNumber)
        {
            var policy = dataStore.Policies.WithNumber(policyNumber);
            
            return policy!=null ? PolicyDtoAssembler.AssemblePolicyDto(policy) : null;
        }
    }
}