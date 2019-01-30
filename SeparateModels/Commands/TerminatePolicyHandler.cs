using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using SeparateModels.Domain;
using SeparateModels.Services;

namespace SeparateModels.Commands
{
    public class TerminatePolicyHandler : IRequestHandler<TerminatePolicyCommand, TerminatePolicyResult>
    {
        private readonly IDataStore dataStore;

        public TerminatePolicyHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<TerminatePolicyResult> Handle(TerminatePolicyCommand command, CancellationToken cancellationToken)
        {
            var policy = dataStore.Policies.WithNumber(command.PolicyNumber);
            policy.TerminatePolicy(command.TerminationDate);
            await dataStore.CommitChanges();
            return new TerminatePolicyResult
            {
                PolicyNumber = policy.Number,
                VersionWithTerminationNumber = policy.Versions.Last().VersionNumber
            };
        }
    }
}