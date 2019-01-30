using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using NoCqrs.Domain;
using NodaMoney;

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


        public BuyAdditionalCoverResult BuyAdditionalCover(BuyAdditionalCoverRequest request)
        {
            var policy = dataStore.Policies.WithNumber(request.PolicyNumber);
            var newCover = policy.Product.Covers.WithCode(request.NewCoverCode);
            policy.ExtendCoverage
            (
                request.EffectiveDateOfChange, 
                new CoverPrice(newCover,Money.Euro(request.NewCoverPrice),request.NewCoverPriceUnit)
            );
            var newPolicyVersion = policy.Versions.Last();
            dataStore.CommitChanges();
            return new BuyAdditionalCoverResult
            {
                PolicyNumber = policy.Number,
                VersionWithAdditionalCoversNumber = newPolicyVersion.VersionNumber
            };
        }

        public ConfirmBuyAdditionalCoverResult ConfirmBuyAdditionalCover(ConfirmBuyAdditionalCoverRequest request)
        {
            var policy = dataStore.Policies.WithNumber(request.PolicyNumber);
            policy.ConfirmChanges(request.VersionToConfirmNumber);
            dataStore.CommitChanges();
            return new ConfirmBuyAdditionalCoverResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            };
        }

        public TerminatePolicyResult TerminatePolicy(TerminatePolicyRequest request)
        {
            var policy = dataStore.Policies.WithNumber(request.PolicyNumber);
            policy.TerminatePolicy(request.TerminationDate);
            dataStore.CommitChanges();
            return new TerminatePolicyResult
            {
                PolicyNumber = policy.Number,
                VersionWithTerminationNumber = policy.Versions.Last().VersionNumber
            };
        }

        public ConfirmTerminationResult ConfirmTermination(ConfirmTerminationRequest request)
        {
            var policy = dataStore.Policies.WithNumber(request.PolicyNumber);
            policy.ConfirmChanges(request.VersionToConfirmNumber);
            dataStore.CommitChanges();
            return new ConfirmTerminationResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            };
        }

        public CancelLastAnnexResult CancelLastAnnex(CancelLastAnnexRequest request)
        {
            var policy = dataStore.Policies.WithNumber(request.PolicyNumber);
            policy.CancelLastAnnex();
            dataStore.CommitChanges();
            return new CancelLastAnnexResult
            {
                PolicyNumber = policy.Number,
                LastActiveVersionNumber = policy.Versions.LatestActive().VersionNumber
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