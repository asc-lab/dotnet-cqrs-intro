using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SeparateModels.Domain;

namespace SeparateModels.Commands
{
    public class ConfirmBuyAdditionalCoverHandler 
        : IRequestHandler<ConfirmBuyAdditionalCoverCommand, ConfirmBuyAdditionalCoverResult>
    {
        private readonly IDataStore dataStore;

        public ConfirmBuyAdditionalCoverHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public Task<ConfirmBuyAdditionalCoverResult> Handle(ConfirmBuyAdditionalCoverCommand command, CancellationToken cancellationToken)
        {
            var policy = dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.ConfirmChanges(command.VersionToConfirmNumber);
            dataStore.CommitChanges();
            return Task.FromResult(new ConfirmBuyAdditionalCoverResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            });
        }
    }
}