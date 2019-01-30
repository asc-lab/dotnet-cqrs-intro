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


        public Task<ConfirmTerminationResult> Handle(ConfirmTerminationCommand command, CancellationToken cancellationToken)
        {
            var policy = dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.ConfirmChanges(command.VersionToConfirmNumber);
            dataStore.CommitChanges();
            return Task.FromResult(new ConfirmTerminationResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            });
        }
    }
}