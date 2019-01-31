using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;

namespace SeparateModels.Commands
{
    public class ConfirmTerminationHandler : IRequestHandler<ConfirmTerminationCommand, ConfirmTerminationResult>
    {
        private readonly IDataStore dataStore;

        public ConfirmTerminationHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }


        public async Task<ConfirmTerminationResult> Handle(ConfirmTerminationCommand command, CancellationToken cancellationToken)
        {
            var policy = await dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.ConfirmChanges(command.VersionToConfirmNumber);
            await dataStore.CommitChanges();
            return new ConfirmTerminationResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            };
        }
    }
}