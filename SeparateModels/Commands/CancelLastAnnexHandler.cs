using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;
using SeparateModels.Domain;
using SeparateModels.Services;

namespace SeparateModels.Commands
{
    public class CancelLastAnnexHandler : IRequestHandler<CancelLastAnnexCommand, CancelLastAnnexResult>
    {
        private readonly IDataStore dataStore;
        private readonly IMediator mediator;
        
        public CancelLastAnnexHandler(IDataStore dataStore, IMediator mediator)
        {
            this.dataStore = dataStore;
            this.mediator = mediator;
        }

        public async Task<CancelLastAnnexResult> Handle(CancelLastAnnexCommand command, CancellationToken cancellationToken)
        {
            var policy = await dataStore.Policies.WithNumber(command.PolicyNumber);
            var lastAnnex = policy.Versions.LatestActive();
            policy.CancelLastAnnex();
            
            await dataStore.CommitChanges();

            await mediator.Publish(new PolicyAnnexCancelled(policy, lastAnnex, policy.Versions.LatestActive()));
            
            return new CancelLastAnnexResult
            {
                PolicyNumber = policy.Number,
                LastActiveVersionNumber = policy.Versions.LatestActive().VersionNumber
            }; 
        }
    }
}