using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;
using SeparateModels.Domain;

namespace SeparateModels.Commands
{
    public class ConfirmBuyAdditionalCoverHandler 
        : IRequestHandler<ConfirmBuyAdditionalCoverCommand, ConfirmBuyAdditionalCoverResult>
    {
        private readonly IDataStore dataStore;
        private readonly IMediator mediator;

        public ConfirmBuyAdditionalCoverHandler(IDataStore dataStore, IMediator mediator)
        {
            this.dataStore = dataStore;
            this.mediator = mediator;
        }

        public async Task<ConfirmBuyAdditionalCoverResult> Handle(ConfirmBuyAdditionalCoverCommand command, CancellationToken cancellationToken)
        {
            using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var policy = await dataStore.Policies.WithNumber(command.PolicyNumber);
                policy.ConfirmChanges(command.VersionToConfirmNumber);
                
                await dataStore.CommitChanges();
                
                await mediator.Publish(new PolicyAnnexed(policy, policy.Versions.WithNumber(command.VersionToConfirmNumber)));
                
                tx.Complete();
                
                return new ConfirmBuyAdditionalCoverResult
                {
                    PolicyNumber = policy.Number,
                    VersionConfirmed = policy.Versions.LatestActive().VersionNumber
                };
            }
        }
    }
}