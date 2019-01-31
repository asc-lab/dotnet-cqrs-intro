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

        public async Task<ConfirmBuyAdditionalCoverResult> Handle(ConfirmBuyAdditionalCoverCommand command, CancellationToken cancellationToken)
        {
            var policy = await dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.ConfirmChanges(command.VersionToConfirmNumber);
            await dataStore.CommitChanges();
            return new ConfirmBuyAdditionalCoverResult
            {
                PolicyNumber = policy.Number,
                VersionConfirmed = policy.Versions.LatestActive().VersionNumber
            };
        }
    }
}