using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using MediatR;
using SeparateModels.Domain;

namespace SeparateModels.Commands
{
    public class ConfirmTerminationHandler : IRequestHandler<ConfirmTerminationCommand, ConfirmTerminationResult>
    {
        private readonly IDataStore dataStore;
        private readonly IMediator mediator;

        public ConfirmTerminationHandler(IDataStore dataStore, IMediator mediator)
        {
            this.dataStore = dataStore;
            this.mediator = mediator;
        }


        public async Task<ConfirmTerminationResult> Handle(ConfirmTerminationCommand command, CancellationToken cancellationToken)
        {
            var policy = await dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.ConfirmChanges(command.VersionToConfirmNumber);
            
            await dataStore.CommitChanges();

            await mediator.Publish(new PolicyTerminated(policy,
                policy.Versions.WithNumber(command.VersionToConfirmNumber)));
            
            return new ConfirmTerminationResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            };
        }
    }
}