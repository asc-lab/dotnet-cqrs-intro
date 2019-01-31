using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;

namespace SeparateModels.Commands
{
    public class CreatePolicyHandler : IRequestHandler<CreatePolicyCommand, CreatePolicyResult>
    {
        private readonly IDataStore dataStore;

        public CreatePolicyHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<CreatePolicyResult> Handle(CreatePolicyCommand command, CancellationToken cancellationToken)
        {
            var offer = await dataStore.Offers.WithNumber(command.OfferNumber);
            var policy = Policy.ConvertOffer(offer, Guid.NewGuid().ToString(), command.PurchaseDate,
                command.PolicyStartDate);
            dataStore.Policies.Add(policy);
            await dataStore.CommitChanges();
            
            return new CreatePolicyResult
            {
                PolicyNumber = policy.Number
            };
        }
    }
}